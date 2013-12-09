namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using global::Common.Mocks;

    public class MockGetAlbumResult : MockServiceResultBase<Album>, IGetAlbumResult
    {
        #region Constructors and Destructors

        public MockGetAlbumResult()
        {
            GetResultFunc = () => new Album();
        }

        #endregion
    }
}