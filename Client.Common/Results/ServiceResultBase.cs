namespace Client.Common.Results
{
    using System;
    using System.Net.Http;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Caliburn.Micro;
    using Client.Common.Services.DataStructures.SubsonicService;

    public abstract class ServiceResultBase<T> : ExtendedResultBase, IServiceResultBase<T>
    {
        #region Fields

        protected readonly HttpClient Client = new HttpClient();

        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        private Action<T> _onSuccess;

        #endregion

        #region Constructors and Destructors

        protected ServiceResultBase(ISubsonicServiceConfiguration configuration)
        {
            Configuration = configuration;
            Response = ResponseFunc;
        }

        #endregion

        #region Public Properties

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public virtual string RequestUrl
        {
            get
            {
                return string.Format(Configuration.RequestFormat(), ViewName);
            }
        }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public T Result { get; set; }

        public abstract string ViewName { get; }

        #endregion

        #region Public Methods and Operators

        public IServiceResultBase<T> OnSuccess(Action<T> onSuccess)
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

        public new IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;

            return this;
        }

        #endregion

        #region Methods

        protected override async Task ExecuteCore(ActionExecutionContext context = null)
        {
            var response = await Response();
            if (response.Exception != null)
            {
                throw response.Exception;
            }

            var xDocument = XDocument.Load(response.Stream);
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

        private async Task<HttpStreamResult> ResponseFunc()
        {
            var result = new HttpStreamResult();
            try
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl);
                httpRequestMessage.Headers.Add(
                    "Authorization", string.Format("Basic {0}", Configuration.EncodedCredentials));
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