namespace Subsonic8.Framework.Services
{
    using SubEchoNest;

    public class EchoNestConfigurationProvider : IConfigurationProvider
    {
        private Configuration _configuration;

        public IConfiguration Configuration
        {
            get
            {
                return _configuration ?? (_configuration = new Configuration
                {
                    ApiKey = EchoNestCredentials.ApiKey,
                    BaseUrl = "http://developer.echonest.com/api/v4"
                });
            }
        }
    }
}
