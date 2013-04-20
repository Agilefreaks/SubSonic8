using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;
using Subsonic8.Shell;
using Windows.UI.Xaml;

namespace Client.Tests.Mocks
{
    public class MockShellViewModel : Screen, IShellViewModel
    {
        public IBottomBarViewModel BottomBar { get; set; }

        public Uri Source { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        public IToastNotificationService NotificationService { get; set; }

        public IDialogNotificationService DialogNotificationService { get; set; }

        public IPlayerControls PlayerControls { get; set; }

        public int PlayPauseCallCount { get; set; }

        public int StopCallCount { get; set; }

        public void PlayNext(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        public void PlayPrevious(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        public void PlayPause()
        {
            PlayPauseCallCount++;
        }

        public void Play(PlaylistItem item)
        {
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Stop()
        {
            StopCallCount++;
        }

        public void SendSearchQueryMessage(string query)
        {
            throw new NotImplementedException();
        }

        public void HandleError(Exception error)
        {
        }

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
    }
}