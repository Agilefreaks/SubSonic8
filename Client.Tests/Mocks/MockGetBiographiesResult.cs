namespace Client.Tests.Mocks
{
    using global::Common.Mocks;
    using SubEchoNest.Models;
    using SubEchoNest.Results;

    public class MockGetBiographiesResult : MockEchoNestResultBase<Biographies>, IGetBiographiesResult
    {
        public string ArtistName { get; private set; }

        public MockGetBiographiesResult(string artistName)
        {
            ArtistName = artistName;
        }
    }
}
