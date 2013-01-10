using System;
using System.IO;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Client.Common.Services;

namespace Client.Common.Results
{
    public abstract class ServiceResultBase<T> : ResultBase, IServiceResultBase<T>
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
        private IErrorHandler _errorHandler;
        private Action<T> _onSuccess;

        protected ServiceResultBase(ISubsonicServiceConfiguration configuration)
        {
            Configuration = configuration;
            Response = ResponseFunc;
        }

        public override async Task Execute(ActionExecutionContext context = null)
        {
            await new VisualStateResult("Loading").Execute();

            var response = await Response();

            if (response.Exception != null)
            {
                OnError(new CommunicationException("Could not connect to the server. Please check the values in the settings panel.\r\n", response.Exception));
                HandleError();
            }
            else
            {
                HandleStreamResponse(response.Stream);
            }

            await new VisualStateResult("LoadingComplete").Execute();
        }

        public virtual void HandleStreamResponse(Stream stream)
        {
            XDocument xDocument = null;
            try
            {
                xDocument = XDocument.Load(stream);
            }
            catch (Exception exception)
            {
                OnError(exception);
                HandleError();
            }

            if (xDocument != null)
            {
                HandleResponse(xDocument);
                OnSuccess();
            }
        }

        public ServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            return this;
        }

        public ServiceResultBase<T> OnSuccess(Action<T> onSuccess)
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

        protected abstract void HandleResponse(XDocument xDocument);

        protected override Task ExecuteCore(ActionExecutionContext context = null)
        {
            return null;
        }

        private async Task<HttpStreamResult> ResponseFunc()
        {
            var result = new HttpStreamResult();
            try
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl);
                httpRequestMessage.Headers.Add("Authorization", string.Format("Basic {0}", Configuration.EncodedCredentials()));
                var response = await Client.SendAsync(httpRequestMessage);
                result.Stream = await response.Content.ReadAsStreamAsync();
            }
            catch (Exception exception)
            {
                result.Exception = exception;
            }

            return result;
        }

        private void HandleError()
        {
            if (_errorHandler != null)
            {
                _errorHandler.HandleError(Error);
            }
        }

        private void OnSuccess()
        {
            if (_onSuccess != null)
            {
                _onSuccess(Result);
            }
        }
    }
}