using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;

namespace Subsonic8.Settings
{
    public class SettingsViewModel : Screen
    {
        private readonly ISubsonicService _subsonicService;
        private readonly IStorageService _storageService;
        private SubsonicServiceConfiguration _configuration;
        private bool _savingInProgress;
        private IDisposable _propertyChangedObserver;

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
                return "Settings";
            }
        }

        public SettingsViewModel(ISubsonicService subsonicService, IStorageService storageService)
        {
            _subsonicService = subsonicService;
            _storageService = storageService;
        }

        public async void SaveSettings()
        {
            _savingInProgress = true;

            await _storageService.Save(Configuration);
            _subsonicService.Configuration = Configuration;

            _savingInProgress = false;
        }

        public async Task Populate()
        {
            Configuration = await _storageService.Load<SubsonicServiceConfiguration>() ??
                            new SubsonicServiceConfiguration();

            _propertyChangedObserver = Observable.FromEventPattern<PropertyChangedEventArgs>(_configuration, "PropertyChanged")
                                                 .Buffer(TimeSpan.FromMilliseconds(400))
                                                 .Where(eventPattern => !_savingInProgress)
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