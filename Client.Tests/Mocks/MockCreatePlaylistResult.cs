using System.Collections.Generic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockCreatePlaylistResult : MockServiceResultBase<bool>, ICreatePlaylistResult
    {
        public string Name { get; private set; }

        public IEnumerable<int> SongIds { get; private set; }
    }
}