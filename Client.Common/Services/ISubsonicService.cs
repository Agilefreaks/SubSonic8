using System;
using Client.Common.Results;

namespace Client.Common.Services
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        Func<IGetRootResult> GetMusicFolders { get; set; }

        Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        Func<int, IGetArtistResult> GetArtist { get; set; }

        Func<int, IGetAlbumResult> GetAlbum { get; set; }

        Func<int, IGetSongResult> GetSong { get; set; }

        Func<string, ISearchResult> Search { get; set; }

        Func<int, IGetIndexResult> GetIndex { get; set; }

        bool HasValidSubsonicUrl { get; }

        Uri GetUriForFileWithId(int id);

        Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitRate = 0);

        Uri GetUriForVideoStartingAt(Uri source, double totalSeconds);

        string GetCoverArtForId(string coverArt);

        string GetCoverArtForId(string coverArt, ImageType imageType);
    }
}