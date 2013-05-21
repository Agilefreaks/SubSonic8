namespace Client.Common.Results
{
    using System.Collections.Generic;

    public interface IUpdatePlaylistResult : IEmptyResponseResult
    {
        #region Public Properties

        int Id { get; }

        IEnumerable<int> SongIdsToAdd { get; }

        IEnumerable<int> SongIndexesToRemove { get; }

        #endregion
    }
}