using SubLastFm;

namespace Subsonic8.Framework.Services
{
    public class LastFmConfigurationProvider : IConfigurationProvider
    {
        private IConfiguration _configuration;

        public IConfiguration Configuration
        {
            get
            {
                return _configuration ?? (_configuration = new Configuration
                {
                    ApiKey = "a7a6e957a0cd7c5e3132a791c8da1f92",
                    BaseUrl = "http://ws.audioscrobbler.com/2.0/",
                });
            }
        }
    }
}