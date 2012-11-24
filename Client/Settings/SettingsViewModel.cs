using System.ComponentModel;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Services;
using WinRtUtility;

namespace Subsonic8.Settings
{
    public class SettingsViewModel : Screen
    {
        private readonly ISubsonicService _subsonicService;
        private readonly ObjectStorageHelper<SubsonicServiceConfiguration> _storageHelper;
        private SubsonicServiceConfiguration _configuration;

        public SubsonicServiceConfiguration Configuration
        {
            get
            {
                return _configuration;
            }

            private set
            {
                _configuration = value;
                NotifyOfPropertyChange();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Credentials";
            }
        }

        public SettingsViewModel(ISubsonicService subsonicService)
        {
            _subsonicService = subsonicService;
            _storageHelper = new ObjectStorageHelper<SubsonicServiceConfiguration>(StorageType.Roaming);
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            Configuration = await _storageHelper.LoadAsync();
            Configuration = Configuration ?? new SubsonicServiceConfiguration();
            _configuration.PropertyChanged += ConfigurationOnPropertyChanged;
        }

        private void ConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveSettings();
        }

        public async void SaveSettings()
        {
            await _storageHelper.SaveAsync(Configuration);
            _subsonicService.Configuration = Configuration;
        }
    }
}