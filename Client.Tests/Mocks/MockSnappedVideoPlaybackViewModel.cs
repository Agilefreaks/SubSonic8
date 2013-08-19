namespace Client.Tests.Mocks
{
    using System;
    using System.ComponentModel;
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Results;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.VideoPlayback;
    using Windows.UI.Xaml;
    using Action = System.Action;

    public class MockSnappedVideoPlaybackViewModel : ISnappedVideoPlaybackViewModel
    {
        public event EventHandler<ActivationEventArgs> Activated;

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<PlaybackStateEventArgs> FullScreenChanged;

        public IToastNotificationService ToastNotificationService { get; private set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan StartTime { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; private set; }

        public bool IsNotifying { get; set; }

        public IErrorHandler ErrorHandler { get; private set; }

        public ISubsonicService SubsonicService { get; set; }

        public bool CanGoBack { get; private set; }

        public IEventAggregator EventAggregator { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IDialogNotificationService NotificationService { get; set; }

        public Action UpdateDisplayName { get; set; }

        public IErrorDialogViewModel ErrorDialogViewModel { get; set; }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play(PlaylistItem item, object options = null)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        void IPlayer.Stop()
        {
            throw new NotImplementedException();
        }

        public PlaybackStateEventArgs GetPlaybackTimeInfo()
        {
            throw new NotImplementedException();
        }

        void IPlaybackControlsViewModel.Stop()
        {
            throw new NotImplementedException();
        }

        public void Activate()
        {
            throw new NotImplementedException();
        }

        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public void TryClose()
        {
            throw new NotImplementedException();
        }

        public void CanClose(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void OnFullScreenChanged()
        {
            throw new NotImplementedException();
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void ClearSource()
        {
            throw new NotImplementedException();
        }
    }
}