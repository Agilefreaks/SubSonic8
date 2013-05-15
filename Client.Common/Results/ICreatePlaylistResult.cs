using System.Collections.Generic;

namespace Client.Common.Results
{
    public interface ICreatePlaylistResult : IServiceResultBase<bool>
    {
        string Name { get; }

        IEnumerable<int> SongIds { get; }
    }
}