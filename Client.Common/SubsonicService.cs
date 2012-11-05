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

        public IGetIndexResult GetRootIndex()
        {
            return new GetIndexResult(_configuration);
        }
    }
}