using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Services
{
    public class SubsonicService : PropertyChangedBase, ISubsonicService
    {
        public const string CoverArtPlaceholder = @"/Assets/CoverArtPlaceholder.jpg";

        private SubsonicServiceConfiguration _configuration;

        public SubsonicServiceConfiguration Configuration
        {
            get
            {
                return _configuration;
            }

            set
            {
                _configuration = value;
                NotifyOfPropertyChange();
            }
        }

        public Func<IGetRootResult> GetMusicFolders { get; set; }

        public Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        public Func<int, IGetArtistResult> GetArtist { get; set; }

        public Func<int, IGetAlbumResult> GetAlbum { get; set; }

        public Func<int, IGetSongResult> GetSong { get; set; }

        public Func<int, IGetIndexResult> GetIndex { set; get; }

        public Func<IGetAllPlaylistsResult> GetAllPlaylists { get; set; }

        public Func<int, IGetPlaylistResult> GetPlaylist { get; set; }

        public Func<string, ISearchResult> Search { get; set; }

        public Func<int, IDeletePlaylistResult> DeletePlaylist { get; set; }

        public Func<string, IEnumerable<int>, ICreatePlaylistResult> CreatePlaylist { get; set; }

        public virtual bool HasValidSubsonicUrl
        {
            get { return Configuration != null && !string.IsNullOrEmpty(Configuration.BaseUrl); }
        }

        public SubsonicService()
        {
            GetMusicFolders = GetMusicFoldersImpl;
            GetMusicDirectory = GetMusicDirectoryImpl;
            GetAlbum = GetAlbumImpl;
            GetArtist = GetArtistImpl;
            GetSong = GetSongImpl;
            GetIndex = GetIndexImpl;
            Search = SearchImpl;
            GetAllPlaylists = GetAllPlaylistsImpl;
            GetPlaylist = GetPlaylistImpl;
            DeletePlaylist = DeletePlaylistImpl;
            CreatePlaylist = CreatePlaylistImpl;
        }

        public virtual Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format(_configuration.RequestFormatWithUsernameAndPassword(), "stream.view", _configuration.Username, _configuration.EncodedPassword) + string.Format("&id={0}", id));
        }

        public virtual Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitRate = 0)
        {
            var uriString = string.Format("{0}stream/stream.ts?id={1}&hls=true&timeOffset={2}", _configuration.BaseUrl, id, timeOffset);
            if (maxBitRate > 0)
            {
                uriString += string.Format("&maxBitRate={0}", maxBitRate);
            }

            return new Uri(uriString);
        }

        public Uri GetUriForVideoStartingAt(Uri source, double totalSeconds)
        {
            var uriString = source.ToString();
            var regex = new Regex("(.*)(?<TIMEOFFSET>&timeOffset=)([0-9]{1,})(.*)");

            var regeExFormat = string.Format("$1${{TIMEOFFSET}}{0}$3", Math.Floor(totalSeconds));

            return new Uri(regex.Replace(uriString, regeExFormat));
        }

        public string GetCoverArtForId(string coverArt)
        {
            return GetCoverArtForId(coverArt, ImageType.Thumbnail);
        }

        public virtual string GetCoverArtForId(string coverArt, ImageType imageType)
        {
            string result;
            if (!string.IsNullOrEmpty(coverArt))
            {
                result =
                    string.Format(_configuration.RequestFormatWithUsernameAndPassword(), "getCoverArt.view", _configuration.Username,
                                  _configuration.EncodedPassword) + string.Format("&id={0}", coverArt) +
                    string.Format("&size={0}", (int)imageType);
            }
            else
            {
                result = CoverArtPlaceholder;
            }

            return result;
        }

        private ISearchResult SearchImpl(string query)
        {
            return new SearchResult(_configuration, query);
        }

        private IGetRootResult GetMusicFoldersImpl()
        {
            return new GetRootResult(_configuration);
        }

        private IGetMusicDirectoryResult GetMusicDirectoryImpl(int id)
        {
            return new GetMusicDirectoryResult(_configuration, id);
        }

        private IGetArtistResult GetArtistImpl(int id)
        {
            return new GetArtistsResult(_configuration, id);
        }

        private IGetAlbumResult GetAlbumImpl(int id)
        {
            return new GetAlbumResult(_configuration, id);
        }

        private IGetIndexResult GetIndexImpl(int musicFolderId)
        {
            return new GetIndexResult(_configuration, musicFolderId);
        }

        private IGetSongResult GetSongImpl(int id)
        {
            return new GetSongResult(_configuration, id);
        }

        private IGetAllPlaylistsResult GetAllPlaylistsImpl()
        {
            return new GetAllPlaylistsResult(_configuration);
        }

        private IGetPlaylistResult GetPlaylistImpl(int id)
        {
            return new GetPlaylistResult(_configuration, id);
        }

        private IDeletePlaylistResult DeletePlaylistImpl(int id)
        {
            return new DeletePlaylistResult(Configuration, id);
        }

        private ICreatePlaylistResult CreatePlaylistImpl(string name, IEnumerable<int> songIds)
        {
            return new CreatePlaylistResult(Configuration, name, songIds);
        }
    }
}