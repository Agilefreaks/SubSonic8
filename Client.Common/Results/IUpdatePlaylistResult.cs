using System.Collections.Generic;

namespace Client.Common.Results
{
    public interface IUpdatePlaylistResult : IServiceResultBase<bool>
    {
        int Id { get; }

        IEnumerable<int> SongIdsToAdd { get; }

        IEnumerable<int> SongIndexesToRemove { get; }
    }
}