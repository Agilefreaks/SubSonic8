using System;
using System.Collections.ObjectModel;
using Subsonic8.BottomBar;

namespace Client.Tests.Mocks
{
    internal class MockDefaultBottomBarViewModel : IDefaultBottomBarViewModel
    {
        public bool IsOpened { get; set; }

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

        public Action Navigate { get; set; }

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

        public bool CanAddToPlaylist { get; private set; }
        public bool CanRemoveFromPlaylist { get; private set; }

        public ObservableCollection<object> SelectedItems { get; set; }
    }
}