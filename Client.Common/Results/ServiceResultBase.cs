using System.Net.Http;

namespace Client.Common.Results
{
    public abstract class ServiceResultBase : ResultBase, IServiceResultBase
    {
        public ISubsonicServiceConfiguration Configuration { get; private set; }

        protected readonly HttpClient Client = new HttpClient();

        protected ServiceResultBase(ISubsonicServiceConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}