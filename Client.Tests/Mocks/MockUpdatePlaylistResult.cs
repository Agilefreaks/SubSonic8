using System.Collections.Generic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockUpdatePlaylistResult : MockServiceResultBase<bool>, IUpdatePlaylistResult
    {
        public int Id { get; private set; }

        public IEnumerable<int> SongIdsToAdd { get; private set; }

        public IEnumerable<int> SongIndexesToRemove { get; private set; }
    }
}