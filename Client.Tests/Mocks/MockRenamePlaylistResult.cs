using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockRenamePlaylistResult : MockServiceResultBase<bool>, IRenamePlaylistResult
    {
        public int Id { get; private set; }

        public string Name { get; private set; }
    }
}