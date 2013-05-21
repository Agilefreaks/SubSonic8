namespace Client.Common.Services.DataStructures.SubsonicService
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
    using Caliburn.Micro;

    public class SubsonicServiceConfiguration : PropertyChangedBase, ISubsonicServiceConfiguration
    {
        #region Static Fields

        private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();

        #endregion

        #region Fields

        private string _baseUrl;

        private string _password;

        private string _username;

        #endregion

        #region Public Properties

        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }

            set
            {
                if (value == _baseUrl)
                {
                    return;
                }

                _baseUrl = AddEndingSlashIfNotExisting(value);
                NotifyOfPropertyChange();
            }
        }

        public string EncodedCredentials
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password));

                return Convert.ToBase64String(bytes);
            }
        }

        public string EncodedPassword
        {
            get
            {
                return "enc:" + BytesToHex(Encoding.UTF8.GetBytes(Password)).ToLowerInvariant();
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
                if (_password == value)
                {
                    return;
                }

                _password = value;
                NotifyOfPropertyChange();
            }
        }

        [XmlIgnore]
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                if (_username == value)
                {
                    return;
                }

                _username = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Methods

        private static string AddEndingSlashIfNotExisting(string value)
        {
            var result = value;
            if (!string.IsNullOrEmpty(value) && result.LastIndexOf("/", StringComparison.Ordinal) != result.Length - 1)
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

        #endregion
    }
}