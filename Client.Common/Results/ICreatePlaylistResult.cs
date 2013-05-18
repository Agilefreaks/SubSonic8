using System.Collections.Generic;

namespace Client.Common.Results
{
    public interface ICreatePlaylistResult : IEmptyResponseResult
    {
        string Name { get; }

        IEnumerable<int> SongIds { get; }
    }
}