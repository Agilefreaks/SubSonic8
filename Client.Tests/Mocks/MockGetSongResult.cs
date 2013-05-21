namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;

    public class MockGetSongResult : MockServiceResultBase<Song>, IGetSongResult
    {
        #region Constructors and Destructors

        public MockGetSongResult(int id = 0)
        {
            GetResultFunc = () => new Song { Id = id };
        }

        #endregion
    }
}