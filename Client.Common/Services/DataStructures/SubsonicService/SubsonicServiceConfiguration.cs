using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace Client.Common.Services.DataStructures.SubsonicService
{
    public class SubsonicServiceConfiguration : PropertyChangedBase, ISubsonicServiceConfiguration
    {
        private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();

        private string _username, _password, _baseUrl;

        [XmlIgnore]
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

        [XmlIgnore]
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

        public string EncodedCredentials
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password));

                return System.Convert.ToBase64String(bytes);   
            }
        }

        public string EncodedPassword
        {
            get
            {
                return "enc:" + BytesToHex(Encoding.UTF8.GetBytes(Password)).ToLowerInvariant();
            }
        }

        private static string AddEndingSlashIfNotExisting(string value)
        {
            var result = value;
            if (!string.IsNullOrEmpty(value) && result.LastIndexOf("/", System.StringComparison.Ordinal) != result.Length - 1)
            {
                result = string.Format("{0}/", result);
            }

            return result;
        }

        private static string BytesToHex(ICollection<byte> data)
        {
            var builder = new StringBuilder(data.Count * 2);
            foreach (var @byte in data)
            {
                builder.Append(HexChars[@byte >> 4]);
                builder.Append(HexChars[@byte & 0xf]);
            }

            return builder.ToString();
        }
    }
}