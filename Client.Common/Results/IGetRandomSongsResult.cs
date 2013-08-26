namespace Client.Common.Results
{
    using System.Collections.Generic;
    using Client.Common.Models.Subsonic;

    public interface IGetRandomSongsResult : IServiceResultBase<IList<Song>>
    {
        int NumberOfSongs { get; } 
    }
}