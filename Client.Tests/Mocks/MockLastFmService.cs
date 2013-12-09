namespace Client.Tests.Mocks
{
    using System;
    using SubLastFm;
    using SubLastFm.Results;

    public class MockLastFmService : LastFmService
    {
        private Func<string, IGetArtistDetailsResult> _getArtistDetailsCallback;

        public int GetArtistDetailsCallCount { get; set; }

        public MockLastFmService(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {
        }

        public override IGetArtistDetailsResult GetArtistDetails(string artistName)
        {
            GetArtistDetailsCallCount++;

            return _getArtistDetailsCallback != null
                ? _getArtistDetailsCallback(artistName)
                : new MockGetArtistDetailsResult(artistName);
        }

        public void SetupGetArtistDetails(Func<string, IGetArtistDetailsResult> callback)
        {
            _getArtistDetailsCallback = callback;
        }
    }
}
