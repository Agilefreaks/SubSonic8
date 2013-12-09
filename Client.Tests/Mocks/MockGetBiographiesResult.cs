namespace Client.Tests.Mocks
{
    using global::Common.Mocks;
    using SubEchoNest.Models;
    using SubEchoNest.Results;

    public class MockGetBiographiesResult : MockEchoNestResultBase<Biographies>, IGetBiographiesResult
    {
        public string ArtistName { get; private set; }

        public MockGetBiographiesResult()
        {
            GetResultFunc = () => new Biographies();
        }

        public MockGetBiographiesResult(string artistName) 
            : this()
        {
            ArtistName = artistName;
        }
    }
}
