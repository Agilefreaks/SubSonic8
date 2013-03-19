using System;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using Client.Common.Results;

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

        public Func<IGetRootResult> GetRootIndex { get; set; }

        public Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        public Func<int, IGetArtistResult> GetArtist { get; set; }

        public Func<int, IGetAlbumResult> GetAlbum { get; set; }

        public Func<int, IGetSongResult> GetSong { get; set; }

        public Func<string, ISearchResult> Search { get; set; }

        public virtual bool IsConfigured
        {
            get { return Configuration != null && !string.IsNullOrEmpty(Configuration.BaseUrl); }
        }

        public SubsonicService()
        {
            GetRootIndex = GetRootIndexImpl;
            GetMusicDirectory = GetMusicDirectoryImpl;
            GetAlbum = GetAlbumImpl;
            GetArtist = GetArtistImpl;
            GetSong = GetSongImpl;
            Search = SearchImpl;
        }

        public virtual Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format(_configuration.RequestFormatWithUsernameAndPassword(), "stream.view", _configuration.Username, _configuration.Password) + string.Format("&id={0}", id));
        }

        public virtual Uri GetUriForVideoWithId(int id, int timeOffset = 0)
        {
            var maxBitRate = 0;
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
                                  _configuration.Password) + string.Format("&id={0}", coverArt) +
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

        private IGetRootResult GetRootIndexImpl()
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

        private IGetSongResult GetSongImpl(int id)
        {
            return new GetSongResult(_configuration, id);
        }
    }
}