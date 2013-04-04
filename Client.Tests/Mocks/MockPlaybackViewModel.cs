using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Action = System.Action;

namespace Client.Tests.Mocks
{
    public class MockPlaybackViewModel : IPlaybackViewModel
    {
        public void Handle(PlaylistMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(PlayFile message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StartVideoPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StartAudioPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StopVideoPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

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
        public int Parameter { get; set; }
        public Uri Source { get; set; }
        public PlaybackViewModelStateEnum State { get; private set; }
        public string CoverArt { get; set; }
        public Func<IId, Task<PlaylistItem>> LoadModel { get; set; }
        public bool ShuffleOn { get; private set; }
        public bool IsPlaying { get; private set; }
        public TimeSpan EndTime { get; set; }
        public void ClearPlaylist()
        {
            throw new NotImplementedException();
        }

        public void SavePlaylist()
        {
            throw new NotImplementedException();
        }

        public IToastNotificationService ToastNotificationService { get; private set; }
    }
}
