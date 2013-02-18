﻿using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.BottomBar;
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

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public Task PerformSubsonicSearch(string query)
        {
            throw new NotImplementedException();
        }

        public void PlayNext(object sender, RoutedEventArgs routedEventArgs)
        {
            throw new NotImplementedException();
        }

        public void PlayPrevious(object sender, RoutedEventArgs routedEventArgs)
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            PlayPauseCallCount++;
        }

        public void Stop()
        {
            StopCallCount++;
        }

        public void HandleError(Exception error)
        {
        }

        public void Handle(StartAudioPlaybackMessage message)
        {
        }

        public void Handle(StopPlaybackMessage message)
        {
        }

        public void Handle(ResumePlaybackMessage message)
        {
        }

        public void Handle(StopAudioPlaybackMessage message)
        {
        }

        public void Handle(PausePlaybackMessage message)
        {
        }
    }
}