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
    }
}