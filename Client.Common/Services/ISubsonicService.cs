using System;
using System.Collections.Generic;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;

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

        Func<IGetAllPlaylistsResult> GetAllPlaylists { get; set; }

        Func<int, IGetPlaylistResult> GetPlaylist { get; set; }

        Func<string, IEnumerable<int>, ICreatePlaylistResult> CreatePlaylist { get; set; }

        Func<int, IEnumerable<int>, IEnumerable<int>, IUpdatePlaylistResult> UpdatePlaylist { get; set; }

        bool HasValidSubsonicUrl { get; }

        Func<int, IDeletePlaylistResult> DeletePlaylist { get; set; }

        Uri GetUriForFileWithId(int id);

        Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitRate = 0);

        Uri GetUriForVideoStartingAt(Uri source, double totalSeconds);

        string GetCoverArtForId(string coverArt);

        string GetCoverArtForId(string coverArt, ImageType imageType);
    }
}