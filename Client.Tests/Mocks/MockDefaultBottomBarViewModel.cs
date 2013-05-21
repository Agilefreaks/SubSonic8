namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.ObjectModel;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Services;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Services;

    public class MockDefaultBottomBarViewModel : IDefaultBottomBarViewModel
    {
        #region Constructors and Destructors

        public MockDefaultBottomBarViewModel()
        {
            CanAddToPlaylist = false;
        }

        #endregion

        #region Public Properties

        public bool CanAddToPlaylist { get; private set; }

        public bool DisplayPlayControls { get; set; }

        public bool IsOpened { get; set; }

        public bool IsPlaying { get; set; }

        public Action NavigateOnPlay { get; set; }

        public IDialogNotificationService NotificationService { get; set; }

        public ObservableCollection<object> SelectedItems { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddToPlaylist()
        {
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            throw new NotImplementedException();
        }

        public void HandleError(Exception error)
        {
        }

        public void NavigateToPlaylist()
        {
        }

        public void NavigateToRoot()
        {
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void PlayAll()
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

        public void PlayPrevious()
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

        #endregion
    }
}