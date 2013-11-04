namespace SubLastFm
{
    using Results;

    public class LastFmService : ILastFmService
    {
        private readonly IConfiguration _configuration;

        public LastFmService(IConfigurationProvider configurationProvider)
        {
            _configuration = configurationProvider.Configuration;
        }

        public IGetArtistDetailsResult GetArtistDetails(string artistName)
        {
            return new GetArtistDetailsResult(_configuration, artistName);
        }
    }
}