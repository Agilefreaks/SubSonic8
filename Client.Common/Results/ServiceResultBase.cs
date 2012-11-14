using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public abstract class ServiceResultBase : ResultBase, IServiceResultBase
    {
        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public abstract string ViewName { get; }

        public Func<Task<XDocument>> Response { get; set; }

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

        public override async void Execute(ActionExecutionContext context)
        {
            try
            {
                var xDocument = await Response();
                HandleResponse(xDocument);
                OnCompleted();
            }
            catch (HttpRequestException e)
            {
                OnError(e);
            }
        }

        protected abstract void HandleResponse(XDocument xDocument);

        private async Task<XDocument> ResponseFunc()
        {
            var response = await Client.GetAsync(RequestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            return XDocument.Load(stream);
        }
    }
}