﻿using Caliburn.Micro;

namespace Client.Common
{
    public class SubsonicServiceConfiguration : PropertyChangedBase
    {
        private string _username;
        private string _password;
        private string _serviceUrl;

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
    }
}