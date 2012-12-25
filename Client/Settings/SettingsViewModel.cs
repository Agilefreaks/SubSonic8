using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.Framework.Services;

namespace Subsonic8.Settings
{
    public class SettingsViewModel : Screen
    {
        private readonly ISubsonicService _subsonicService;
        private readonly INotificationService _notificationService;
        private readonly IStorageService _storageService;
        private Subsonic8Configuration _configuration;
        private IDisposable _propertyChangedObserver;
        private bool _savingInProgress;

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

        public async void SaveSettings()
        {
            _savingInProgress = true;

            await _storageService.Save(Configuration);

            _subsonicService.Configuration = Configuration.SubsonicServiceConfiguration;
            _notificationService.UseSound = Configuration.ToastsUseSound;
            _savingInProgress = false;
        }

        public async Task Populate()
        {
            Configuration = await _storageService.Load<Subsonic8Configuration>() ??
                            new Subsonic8Configuration { ToastsUseSound = false };

            _propertyChangedObserver = Observable.FromEventPattern<PropertyChangedEventArgs>(_configuration, "PropertyChanged")
                                                 .Buffer(TimeSpan.FromMilliseconds(400))
                                                 .Where(eventPattern => eventPattern.Count > 0 && !_savingInProgress)
                                                 .Subscribe(eventPattern => SaveSettings());
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            await Populate();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _propertyChangedObserver.Dispose();
        }
    }
}