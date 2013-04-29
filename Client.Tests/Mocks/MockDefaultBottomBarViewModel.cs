using System;
using System.Collections.ObjectModel;
using Client.Common.EventAggregatorMessages;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockDefaultBottomBarViewModel : IDefaultBottomBarViewModel
    {
        public ObservableCollection<object> SelectedItems { get; set; }

        public bool IsOpened { get; set; }

        public bool IsPlaying { get; set; }

        public Action NavigateOnPlay { get; set; }

        public bool DisplayPlayControls { get; set; }

        public bool CanAddToPlaylist { get; private set; }
        public IDialogNotificationService NotificationService { get; set; }

        public MockDefaultBottomBarViewModel()
        {
            CanAddToPlaylist = false;
        }

        public void NavigateToPlaylist()
        {
        }

        public void PlayPrevious()
        {
            throw new NotImplementedException();
        }

        public void PlayNext()
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void ToggleShuffle()
        {
        }

        public void NavigateToRoot()
        {
        }

        public void AddToPlaylist()
        {

        }

        public void PlayAll()
        {
            throw new NotImplementedException();
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            throw new NotImplementedException();
        }

        public void HandleError(Exception error)
        {
        }

        public ISubsonicService SubsonicService { get; set; }
    }
}