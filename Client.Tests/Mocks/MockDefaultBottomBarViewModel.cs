using System;
using System.Collections.ObjectModel;
using Subsonic8.BottomBar;
using Subsonic8.Messages;

namespace Client.Tests.Mocks
{
    public class MockDefaultBottomBarViewModel : IDefaultBottomBarViewModel
    {
        public ObservableCollection<object> SelectedItems { get; set; }

        public bool IsOpened { get; set; }

        public bool IsPlaying { get; set; }

        public Action Navigate { get; set; }

        public bool DisplayPlayControls { get; set; }

        public bool CanAddToPlaylist { get; private set; }

        public bool CanRemoveFromPlaylist { get; private set; }

        public void NavigateToPlaylist()
        {

        }

        public void PlayPrevious()
        {
            throw new System.NotImplementedException();
        }

        public void PlayNext()
        {
            throw new System.NotImplementedException();
        }

        public void PlayPause()
        {
            throw new System.NotImplementedException();
        }

        public void Play()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void AddToPlaylist()
        {

        }

        public void PlayAll()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFromPlaylist()
        {
            
        }

        public void Handle(ShowControlsMessage message)
        {
            throw new NotImplementedException();
        }
    }
}