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
                    ApiKey = LastFmCredentials.ApiKey,
                    BaseUrl = "http://ws.audioscrobbler.com/2.0/",
                });
            }
        }
    }
}