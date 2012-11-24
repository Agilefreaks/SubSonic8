using System;
using System.IO;
using System.Net.Http;
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

        public Func<Task<Stream>> Response { get; set; }

        public virtual string RequestUrl
        {
            get
            {
                return string.Format(Configuration.ServiceUrl, ViewName, Configuration.Username, Configuration.Password);
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
            try
            {
                await new VisualStateResult("Loading").Execute();

                var stream = await Response();
                HandleStreamResponse(stream);

                await new VisualStateResult("LoadingComplete").Execute();
            }
            catch (HttpRequestException e)
            {
                OnError(e);
            }
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

        private async Task<Stream> ResponseFunc()
        {
            var response = await Client.GetAsync(RequestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            return stream;
        }
    }
}