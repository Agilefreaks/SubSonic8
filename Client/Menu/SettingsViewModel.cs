using System.ComponentModel;
using Caliburn.Micro;
using Client.Common;
using WinRtUtility;

namespace Subsonic8.Menu
{
    public class SettingsViewModel : Screen
    {
        private readonly SubsonicService _subsonicService;
        private readonly ObjectStorageHelper<SubsonicServiceConfiguration> _storageHelper;
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

        public SettingsViewModel(SubsonicService subsonicService)
        {
            _subsonicService = subsonicService;
            DisplayName = "Credentials";
            _storageHelper = new ObjectStorageHelper<SubsonicServiceConfiguration>(StorageType.Roaming);
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            Configuration = await _storageHelper.LoadAsync();
            Configuration = Configuration ?? new SubsonicServiceConfiguration();
            Configuration.PropertyChanged += ConfigurationOnPropertyChanged;
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