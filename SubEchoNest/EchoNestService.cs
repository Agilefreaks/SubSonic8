namespace SubEchoNest
{
    using SubEchoNest.Results;

    public class EchoNestService : IEchoNestService
    {
        private readonly IConfiguration _configuration;

        public EchoNestService(IConfigurationProvider configurationProvider)
        {
            _configuration = configurationProvider.Configuration;
        }

        public virtual IGetBiographiesResult GetArtistBiographies(string artistName)
        {
            return new GetBiographiesResult(_configuration, artistName);
        }
    }
}
