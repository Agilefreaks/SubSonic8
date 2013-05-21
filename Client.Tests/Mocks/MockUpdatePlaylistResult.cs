namespace Client.Tests.Mocks
{
    using System.Collections.Generic;
    using Client.Common.Results;

    public class MockUpdatePlaylistResult : MockServiceResultBase<bool>, IUpdatePlaylistResult
    {
        #region Public Properties

        public int Id { get; private set; }

        public IEnumerable<int> SongIdsToAdd { get; private set; }

        public IEnumerable<int> SongIndexesToRemove { get; private set; }

        #endregion
    }
}