namespace Client.Tests.Mocks
{
    using System;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;
    using Subsonic8.Shell;
    using Windows.UI.Xaml;

    public class MockShellViewModel : Screen, IShellViewModel
    {
        #region Public Properties

        public IBottomBarViewModel BottomBar { get; set; }

        public IDialogNotificationService DialogNotificationService { get; set; }

        public IToastNotificationService NotificationService { get; set; }

        public int PlayPauseCallCount { get; set; }

        public IPlayerControls PlayerControls { get; set; }

        public Uri Source { get; set; }

        public int StopCallCount { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Handle(StartPlaybackMessage message)
        {
        }

        public void Handle(StopMessage message)
        {
        }

        public void Handle(ResumePlaybackMessage message)
        {
        }

        public void Handle(StopPlaybackMessage message)
        {
        }

        public void Handle(PauseMessage message)
        {
        }

        public void Handle(ChangeBottomBarMessage message)
        {
        }

        public async Task HandleError(Exception error)
        {
            await Task.Run(() => { });
        }

        public void Pause()
        {
        }

        public void Play(PlaylistItem item, object options = null)
        {
        }

        public void PlayNext(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        public void PlayPause()
        {
            PlayPauseCallCount++;
        }

        public void PlayPrevious(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        public void Resume()
        {
        }

        public void SendSearchQueryMessage(string query)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            StopCallCount++;
        }

        #endregion
    }
}