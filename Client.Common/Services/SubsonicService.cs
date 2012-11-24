using System;
using Caliburn.Micro;
using Client.Common.Results;

namespace Client.Common.Services
{
    public class SubsonicService : PropertyChangedBase, ISubsonicService
    {
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

        public Func<int, IGetAlbumResult> GetAlbum { get; set; }

        public Func<string, ISearchResult> Search { get; set; }

        public Func<int, IGetArtistResult> GetArtist { get; set; }

        public SubsonicService()
        {
            GetRootIndex = GetRootIndexImpl;
            GetMusicDirectory = GetMusicDirectoryImpl;
            GetAlbum = GetAlbumImpl;
            GetArtist = GetArtistImpl;
            Search = SearchImpl;
        }

        public virtual Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format(_configuration.ServiceUrl, "stream.view", _configuration.Username, _configuration.Password) + string.Format("&id={0}", id));
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

        private IGetAlbumResult GetAlbumImpl(int id)
        {
            return new GetAlbumResult(_configuration, id);
        }

        private IGetArtistResult GetArtistImpl(int id)
        {
            return new GetArtistsResult(_configuration, id);
        }
    }
}