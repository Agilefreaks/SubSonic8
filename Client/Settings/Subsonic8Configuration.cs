namespace Subsonic8.Settings
{
    using System.ComponentModel;
    using Caliburn.Micro;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class Subsonic8Configuration : PropertyChangedBase
    {
        #region Fields

        private SubsonicServiceConfiguration _subsonicServiceConfiguration;

        private bool _toastsUseSound;

        #endregion

        #region Constructors and Destructors

        public Subsonic8Configuration()
        {
            SubsonicServiceConfiguration = new SubsonicServiceConfiguration();
        }

        #endregion

        #region Public Properties

        public SubsonicServiceConfiguration SubsonicServiceConfiguration
        {
            get
            {
                return _subsonicServiceConfiguration;
            }

            set
            {
                if (Equals(value, _subsonicServiceConfiguration))
                {
                    return;
                }

                HookChildObject(value, _subsonicServiceConfiguration);
                _subsonicServiceConfiguration = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ToastsUseSound
        {
            get
            {
                return _toastsUseSound;
            }

            set
            {
                if (value.Equals(_toastsUseSound))
                {
                    return;
                }

                _toastsUseSound = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Methods

        private void HookChildObject(INotifyPropertyChanged newValue, INotifyPropertyChanged oldValue)
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

        private void SubsonicServiceConfigurationChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyOfPropertyChange(() => SubsonicServiceConfiguration);
        }

        #endregion
    }
}