namespace Client.Tests.Mocks
{
    using System;
    using SubEchoNest;
    using SubEchoNest.Results;

    public class MockEchoNestService : IEchoNestService
    {
        private Func<string, IGetBiographiesResult> _getBiographiesCallback;

        public int GetArtistBiographiesCallCount { get; set; }

        public IGetBiographiesResult GetArtistBiographies(string artistName)
        {
            GetArtistBiographiesCallCount++;

            return _getBiographiesCallback != null
                ? _getBiographiesCallback(artistName)
                : new MockGetBiographiesResult(artistName);
        }

        public void SetupGetArtistBiographies(Func<string, IGetBiographiesResult> callback)
        {
            _getBiographiesCallback = callback;
        }
    }
}
