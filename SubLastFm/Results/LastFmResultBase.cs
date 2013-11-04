namespace SubLastFm.Results
{
    using System;
    using System.Net.Http;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Results;
    using SubLastFm.Models;

    public abstract class LastFmResultBase<T> : ExtendedResultBase, ILastFmResultBase<T>
    {
        #region Fields

        protected readonly HttpClient Client =
            new HttpClient(new HttpClientHandler { AllowAutoRedirect = true, PreAuthenticate = true });

        // ReSharper disable StringLiteralTypo
        private const string WrapperElementName = "lfm";
        // ReSharper restore StringLiteralTypo
        private Action<T> _onSuccess;

        #endregion

        #region Constructors and Destructors

        protected LastFmResultBase(IConfiguration configuration)
        {
            Configuration = configuration;
            Response = ResponseFunc;
        }

        #endregion

        #region Public Properties

        public IConfiguration Configuration { get; private set; }

        public virtual string RequestUrl
        {
            get
            {
                return string.Format(
                    Configuration.RequestFormatWithApiKey, Configuration.BaseUrl, Configuration.ApiKey, MethodName);
            }
        }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public T Result { get; set; }

        public abstract string MethodName { get; }

        #endregion

        #region Public Methods and Operators

        public ILastFmResultBase<T> OnSuccess(Action<T> onSuccess)
        {
            if (_onSuccess != null)
            {
                var oldOnSuccess = _onSuccess;
                _onSuccess = result =>
                    {
                        oldOnSuccess(result);
                        onSuccess(result);
                    };
            }
            else
            {
                _onSuccess = onSuccess;
            }

            return this;
        }

        public new ILastFmResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;

            return this;
        }

        #endregion

        #region Methods

        protected override async Task ExecuteCore()
        {
            var response = await Response();
            if (response.Exception != null)
            {
                throw response.Exception;
            }

            var xDocument = XDocument.Load(response.Stream);

            HandleFailedCall(xDocument);
            HandleResponse(xDocument);
        }

        protected override void ExecuteOnSuccessAction()
        {
            base.ExecuteOnSuccessAction();
            if (_onSuccess != null)
            {
                _onSuccess(Result);
            }
        }

        protected abstract void HandleResponse(XDocument xDocument);

        private void HandleFailedCall(XDocument xDocument)
        {
            var xElement = xDocument.Element(WrapperElementName);
            var xmlSerializer = new XmlSerializer(typeof(LastFmResponse), new[] { typeof(Error) });
            using (var xmlReader = xElement.CreateReader())
            {
                var response = (LastFmResponse)xmlSerializer.Deserialize(xmlReader);
                if (response.Status == LastFmResponseStatusEnum.Failed)
                {
                    throw new ApiException(response.Error);
                }
            }
        }

        private async Task<HttpStreamResult> ResponseFunc()
        {
            var result = new HttpStreamResult();
            try
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl);
                var response = await Client.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    result.Stream = await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    throw new CommunicationException(
                        string.Format(
                            "Response was:\r\nStatus Code: {0}\r\nReason: {1}", response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (HttpRequestException exception)
            {
                var innerMessage = exception.InnerException != null
                                       ? exception.Message + "\r\n" + exception.InnerException.Message
                                       : exception.Message;
                result.Exception =
                    new CommunicationException(
                        string.Format("Could not perform Http request.\r\nMessage:\r\n{0}", innerMessage), exception);
            }
            catch (Exception exception)
            {
                result.Exception = exception;
            }

            return result;
        }

        #endregion
    }
}