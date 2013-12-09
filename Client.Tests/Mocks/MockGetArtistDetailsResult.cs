namespace Client.Tests.Mocks
{
    using SubLastFm.Models;
    using SubLastFm.Results;

    public class MockGetArtistDetailsResult : MockLastFmResultBase<ArtistDetails>, IGetArtistDetailsResult
    {
        public string ArtistName { get; set; }

        public MockGetArtistDetailsResult()
        {
            GetResultFunc = () => new ArtistDetails();
        }

        public MockGetArtistDetailsResult(string artistName)
            :this()
        {
            ArtistName = artistName;
        }
    }
}
