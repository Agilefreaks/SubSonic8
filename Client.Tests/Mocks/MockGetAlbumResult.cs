using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockGetAlbumResult : MockServiceResultBase<Common.Models.Subsonic.Album>, IGetAlbumResult
    {
        public MockGetAlbumResult()
        {
            GetResultFunc = () => new Common.Models.Subsonic.Album();
        }
    }
}