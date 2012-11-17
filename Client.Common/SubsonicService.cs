using System;
using Caliburn.Micro;
using Client.Common.Results;

namespace Client.Common
{
    public class SubsonicService : PropertyChangedBase, ISubsonicService
    {
        private SubsonicServiceConfiguration _configuration;

        public Func<IGetRootResult> GetRootIndex { get; set; }

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

        public Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        public SubsonicService()
        {
            GetRootIndex = GetRootIndexImpl;
            GetMusicDirectory = GetMusicDirectoryImpl;
        }

        public virtual Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format(_configuration.ServiceUrl, "stream.view", _configuration.Username, _configuration.Password) + string.Format("&id={0}", id));
        }

        private IGetRootResult GetRootIndexImpl()
        {
            return new GetRootResult(_configuration);
        }

        private IGetMusicDirectoryResult GetMusicDirectoryImpl(int id)
        {
            return new GetMusicDirectoryResult(_configuration, id);
        }
    }
}