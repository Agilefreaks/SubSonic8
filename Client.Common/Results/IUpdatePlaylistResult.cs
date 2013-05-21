using System.Collections.Generic;

namespace Client.Common.Results
{
    public interface IUpdatePlaylistResult : IEmptyResponseResult
    {
        int Id { get; }

        IEnumerable<int> SongIdsToAdd { get; }

        IEnumerable<int> SongIndexesToRemove { get; }
    }
}