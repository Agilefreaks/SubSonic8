using System;
using Caliburn.Micro;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Services;
using Action = System.Action;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class ViewModelBase : Screen, IViewModel
    {
        private ICustomFrameAdapter _navigationService;
        private ISubsonicService _subsonicService;
        private IEventAggregator _eventAggregator;

        [Inject]
        public ICustomFrameAdapter NavigationService
        {
            get
            {
                return _navigationService;
            }

            set
            {
                _navigationService = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanGoBack);
            }
        }

        [Inject]
        public ISubsonicService SubsonicService
        {
            get
            {
                return _subsonicService;
            }

            set
            {
                _subsonicService = value;
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        [Inject]
        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }

            set
            {
                _eventAggregator = value;
                OnEventAggregatorSet();
            }
        }

        public bool CanGoBack
        {
            get
            {
                return NavigationService != null && NavigationService.CanGoBack;
            }
        }

        public void GoBack()
        {
            NavigationService.GoBack();
        }

        public Action UpdateDisplayName { get; set; }

        protected ViewModelBase()
        {
            UpdateDisplayName = () => DisplayName = "Subsonic8";
        }

        public async void HandleError(Exception error)
        {
            await NotificationService.Show(new DialogNotificationOptions
                {
                    Message = error.ToString(),
                });
        }

        protected virtual void OnEventAggregatorSet()
        {
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            UpdateDisplayName();
        }
    }
}