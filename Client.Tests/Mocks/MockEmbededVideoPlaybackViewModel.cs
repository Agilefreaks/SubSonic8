using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.VideoPlayback;
using Action = System.Action;

namespace Client.Tests.Mocks
{
    public class MockEmbededVideoPlaybackViewModel : IEmbededVideoPlaybackViewModel
    {
        public string DisplayName { get; set; }
        public void Activate()
        {
            throw new NotImplementedException();
        }

        public bool IsActive { get; private set; }
        public event EventHandler<ActivationEventArgs> Activated;
        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Deactivated;
        public void TryClose()
        {
            throw new NotImplementedException();
        }

        public void CanClose(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public bool IsNotifying { get; set; }
        public void HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public IEventAggregator EventAggregator { get; set; }
        public ICustomFrameAdapter NavigationService { get; set; }
        public ISubsonicService SubsonicService { get; set; }
        public IDialogNotificationService NotificationService { get; set; }
        public IDefaultBottomBarViewModel BottomBar { get; set; }
        public ObservableCollection<object> SelectedItems { get; private set; }
        public bool CanGoBack { get; private set; }
        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public Action UpdateDisplayName { get; set; }
        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public IToastNotificationService ToastNotificationService { get; private set; }
        public void Handle(StartVideoPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StopVideoPlaybackMessage message)
        {
            throw new NotImplementedException();
        }
    }
}