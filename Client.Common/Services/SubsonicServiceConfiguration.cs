using Caliburn.Micro;

namespace Client.Common.Services
{
    public class SubsonicServiceConfiguration : PropertyChangedBase, ISubsonicServiceConfiguration
    {
        private string _username, _password, _serviceUrl;
        private string _baseUrl;

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                NotifyOfPropertyChange();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                NotifyOfPropertyChange();
            }
        }

        public string ServiceUrl
        {
            get
            {
                return _serviceUrl;
            }

            set
            {
                _serviceUrl = value;
                NotifyOfPropertyChange();
            }
        }

        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }

            set
            {
                if (value == _baseUrl) return;
                _baseUrl = value;
                NotifyOfPropertyChange();
            }
        }
    }
}