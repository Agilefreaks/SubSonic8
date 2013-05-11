using Client.Common.Models.Subsonic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockGetAllPlaylistsResult : MockServiceResultBase<PlaylistCollection>, IGetAllPlaylistsResult
    {
    }
}