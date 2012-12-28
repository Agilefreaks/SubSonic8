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
                return string.Format(Configuration.RequestFormat(), ViewName, Configuration.Username, Configuration.Password);
            }
        }

        protected readonly HttpClient Client = new HttpClient();

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
            }

            if (xDocument != null)
            {
                HandleResponse(xDocument);
            }
        }

        protected abstract void HandleResponse(XDocument xDocument);

        protected override Task ExecuteCore(ActionExecutionContext context = null)
        {
            return null;
        }

        protected override async void OnError(Exception error)
        {
            base.OnError(error);
            await new MessageDialogResult(error.ToString(), "Ooops...").Execute();
        }

        private async Task<HttpStreamResult> ResponseFunc()
        {
            var result = new HttpStreamResult();
            try
            {
                var response = await Client.GetAsync(RequestUrl);
                result.Stream = await response.Content.ReadAsStreamAsync();
            }
            catch (Exception exception)
            {
                result.Exception = exception;
            }

            return result;
        }
    }
}