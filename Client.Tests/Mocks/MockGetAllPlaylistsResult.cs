namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;

    public class MockGetAllPlaylistsResult : MockServiceResultBase<PlaylistCollection>, IGetAllPlaylistsResult
    {
    }
}