using System;
using Client.Common.Results;

namespace Client.Common.Services
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        Func<IGetRootResult> GetRootIndex { get; set; }

        Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        Func<int, IGetAlbumResult> GetAlbum { get; set; }

        Func<int, IGetArtistResult> GetArtist { get; set; }

        Func<string, ISearchResult> Search { get; set; }

        Uri GetUriForFileWithId(int id);
    }
}