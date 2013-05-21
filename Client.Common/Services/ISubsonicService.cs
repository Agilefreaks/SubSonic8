namespace Client.Common.Services
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;

    public interface ISubsonicService
    {
        #region Public Properties

        SubsonicServiceConfiguration Configuration { get; set; }

        Func<string, IEnumerable<int>, ICreatePlaylistResult> CreatePlaylist { get; set; }

        Func<int, IDeletePlaylistResult> DeletePlaylist { get; set; }

        Func<int, IGetAlbumResult> GetAlbum { get; set; }

        Func<IGetAllPlaylistsResult> GetAllPlaylists { get; set; }

        Func<int, IGetArtistResult> GetArtist { get; set; }

        Func<int, IGetIndexResult> GetIndex { get; set; }

        Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        Func<IGetRootResult> GetMusicFolders { get; set; }

        Func<int, IGetPlaylistResult> GetPlaylist { get; set; }

        Func<int, IGetSongResult> GetSong { get; set; }

        bool HasValidSubsonicUrl { get; }

        Func<IPingResult> Ping { get; set; }

        Func<int, string, IRenamePlaylistResult> RenamePlaylist { get; set; }

        Func<string, ISearchResult> Search { get; set; }

        Func<int, IEnumerable<int>, IEnumerable<int>, IUpdatePlaylistResult> UpdatePlaylist { get; set; }

        #endregion

        #region Public Methods and Operators

        string GetCoverArtForId(string coverArt);

        string GetCoverArtForId(string coverArt, ImageType imageType);

        Uri GetUriForFileWithId(int id);

        Uri GetUriForVideoStartingAt(Uri source, double totalSeconds);

        Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitRate = 0);

        #endregion
    }
}