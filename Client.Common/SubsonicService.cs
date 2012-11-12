using System;
using Caliburn.Micro;
using Client.Common.Results;

namespace Client.Common
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

        public IGetRootResult GetRootIndex()
        {
            return new GetRootResult(_configuration);
        }

        public IGetMusicDirectoryResult GetMusicDirectory(int id)
        {
            return new GetMusicDirectoryResult(_configuration, id);
        }

        public Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format(_configuration.ServiceUrl, "stream.view", _configuration.Username, _configuration.Password) + string.Format("&id={0}", id));
        }
    }
}