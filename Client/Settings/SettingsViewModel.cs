using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;
using Subsonic8.Main;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Settings
{
    public class SettingsViewModel : Screen
    {
        private readonly ISubsonicService _subsonicService;
        private readonly IToastNotificationService _notificationService;
        private readonly IStorageService _storageService;
        private readonly ICustomFrameAdapter _navigationService;
        private Subsonic8Configuration _configuration;

        public Subsonic8Configuration Configuration
        {
            get
            {
                return _configuration;
            }

            private set
            {
                _configuration = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSaveChanges);
            }
        }

        public bool CanSaveChanges
        {
            get
            {
                return Configuration != null &&
                       !string.IsNullOrWhiteSpace(Configuration.SubsonicServiceConfiguration.Username) &&
                       !string.IsNullOrWhiteSpace(Configuration.SubsonicServiceConfiguration.Password);
            }
        }

        [Inject]
        public ISettingsHelper SettingsHelper { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Settings";
            }
        }

        public SettingsViewModel(ISubsonicService subsonicService, IToastNotificationService notificationService,
            IStorageService storageService, ICustomFrameAdapter navigationService)
        {
            _subsonicService = subsonicService;
            _notificationService = notificationService;
            _storageService = storageService;
            _navigationService = navigationService;
        }

        public async void SaveChanges()
        {
            await SaveSettings();
            _navigationService.NavigateToViewModel<MainViewModel>(true);
        }

        public void UsernameChanged(TextBox textBox)
        {
            Configuration.SubsonicServiceConfiguration.Username = textBox.Text;
            NotifyOfPropertyChange(() => CanSaveChanges);
        }

        public void PasswordChanged(PasswordBox passwordBox)
        {
            Configuration.SubsonicServiceConfiguration.Password = passwordBox.Password;
            NotifyOfPropertyChange(() => CanSaveChanges);
        }

        public async Task Populate()
        {
            var configuration = await _storageService.Load<Subsonic8Configuration>() ??
                                new Subsonic8Configuration { ToastsUseSound = false };
            PopulateCredentials(configuration);
            Configuration = configuration;
        }

        public async Task SaveSettings()
        {
            await _storageService.Save(Configuration);
            UpdateCredentials();

            _subsonicService.Configuration = Configuration.SubsonicServiceConfiguration;
            _notificationService.UseSound = Configuration.ToastsUseSound;
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            await Populate();
        }

        private void PopulateCredentials(Subsonic8Configuration configuration)
        {
            var credentialsFromVault = SettingsHelper.GetCredentialsFromVault();
            if (credentialsFromVault == null) return;
            configuration.SubsonicServiceConfiguration.Username = credentialsFromVault.UserName;
            configuration.SubsonicServiceConfiguration.Password = credentialsFromVault.Password;
        }

        private void UpdateCredentials()
        {
            var passwordCredential = new PasswordCredential
                {
                    UserName = Configuration.SubsonicServiceConfiguration.Username,
                    Password = Configuration.SubsonicServiceConfiguration.Password,
                    Resource = Framework.SettingsHelper.PasswordVaultResourceName
                };

            SettingsHelper.UpdateCredentialsInVault(passwordCredential);
        }
    }
}