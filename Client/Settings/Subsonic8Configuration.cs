using System.ComponentModel;
using Caliburn.Micro;
using Client.Common.Services;

namespace Subsonic8.Settings
{
    public class Subsonic8Configuration : PropertyChangedBase
    {
        private SubsonicServiceConfiguration _subsonicServiceConfiguration;
        private bool _toastsUseSound;

        public bool ToastsUseSound
        {
            get
            {
                return _toastsUseSound;
            }

            set
            {
                if (value.Equals(_toastsUseSound)) return;
                _toastsUseSound = value;
                NotifyOfPropertyChange();
            }
        }

        public SubsonicServiceConfiguration SubsonicServiceConfiguration
        {
            get
            {
                return _subsonicServiceConfiguration;
            }

            set
            {
                if (Equals(value, _subsonicServiceConfiguration)) return;
                HookChildObject(value, _subsonicServiceConfiguration);
                _subsonicServiceConfiguration = value;
                NotifyOfPropertyChange();
            }
        }

        public Subsonic8Configuration()
        {
            SubsonicServiceConfiguration = new SubsonicServiceConfiguration();
        }

        private void HookChildObject(SubsonicServiceConfiguration newValue, SubsonicServiceConfiguration oldValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= SubsonicServiceConfigurationChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += SubsonicServiceConfigurationChanged;
            }
        }

        private void SubsonicServiceConfigurationChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyOfPropertyChange(() => SubsonicServiceConfiguration);
        }
    }
}