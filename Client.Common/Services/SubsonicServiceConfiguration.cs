using Caliburn.Micro;

namespace Client.Common.Services
{
    public class SubsonicServiceConfiguration : PropertyChangedBase, ISubsonicServiceConfiguration
    {
        private string _username, _password, _baseUrl;

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

        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }

            set
            {
                if (value == _baseUrl) return;
                _baseUrl = AddEndingSlashIfNotExisting(value);
                NotifyOfPropertyChange();
            }
        }

        public string EncodedCredentials()
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password));
            return System.Convert.ToBase64String(bytes);
        }

        private string AddEndingSlashIfNotExisting(string value)
        {
            var result = value;
            if (!string.IsNullOrEmpty(value) && result.LastIndexOf("/", System.StringComparison.Ordinal) != result.Length - 1)
            {
                result = string.Format("{0}/", result);
            }

            return result;
        }
    }
}