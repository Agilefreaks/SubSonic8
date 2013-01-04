using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.Framework.Services;
using Subsonic8.Main;

namespace Subsonic8.Settings
{
    public class SettingsViewModel : Screen
    {
        private readonly ISubsonicService _subsonicService;
        private readonly INotificationService _notificationService;
        private readonly IStorageService _storageService;
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
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Settings";
            }
        }

        public SettingsViewModel(ISubsonicService subsonicService, INotificationService notificationService,
            IStorageService storageService)
        {
            _subsonicService = subsonicService;
            _notificationService = notificationService;
            _storageService = storageService;
        }

        public async Task SaveSettings()
        {
            await _storageService.Save(Configuration);

            _subsonicService.Configuration = Configuration.SubsonicServiceConfiguration;
            _notificationService.UseSound = Configuration.ToastsUseSound;
        }

        public async Task Populate()
        {
            Configuration = await _storageService.Load<Subsonic8Configuration>() ??
                            new Subsonic8Configuration { ToastsUseSound = false };
        }

        public async void SaveChanges()
        {
            await SaveSettings();
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            await Populate();
        }
    }
}