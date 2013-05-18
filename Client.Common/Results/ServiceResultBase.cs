using System;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public abstract class ServiceResultBase<T> : ExtendedResultBase, IServiceResultBase<T>
    {
        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public T Result { get; set; }

        public abstract string ViewName { get; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public virtual string RequestUrl
        {
            get
            {
                return string.Format(Configuration.RequestFormat(), ViewName);
            }
        }

        protected readonly HttpClient Client = new HttpClient();
        private Action<T> _onSuccess;

        protected ServiceResultBase(ISubsonicServiceConfiguration configuration)
        {
            Configuration = configuration;
            Response = ResponseFunc;
        }

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


        protected abstract void HandleResponse(XDocument xDocument);

        protected override async Task ExecuteCore(ActionExecutionContext context = null)
        {
            var response = await Response();
            if (response.Exception != null)
            {
                throw new CommunicationException("Could not complete request.\r\n", response.Exception);
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

        private async Task<HttpStreamResult> ResponseFunc()
        {
            var result = new HttpStreamResult();
            try
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl);
                httpRequestMessage.Headers.Add("Authorization", string.Format("Basic {0}", Configuration.EncodedCredentials));
                var response = await Client.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    result.Stream = await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    throw new CommunicationException(
                        string.Format("Response was:\r\nStatus Code:{0}\r\nReason:{1}", response.StatusCode,
                                      response.ReasonPhrase));
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
            }

            return result;
        }
    }
}