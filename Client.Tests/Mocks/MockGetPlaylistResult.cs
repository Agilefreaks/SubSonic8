using Client.Common.Models.Subsonic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockGetPlaylistResult : MockServiceResultBase<Playlist>, IGetPlaylistResult
    {
        public int Id { get; private set; }
    }
}