namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Services;
    using Subsonic8.Playback;
    using Action = System.Action;

    public class MockPlaybackViewModel : IPlaybackViewModel
    {
        #region Public Events

        public event EventHandler<ActivationEventArgs> Activated;

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public object ActiveItem { get; private set; }

        public IDefaultBottomBarViewModel BottomBar { get; set; }

        public bool CanGoBack { get; private set; }

        public string CoverArt { get; set; }

        public string DisplayName { get; set; }

        public TimeSpan EndTime { get; set; }

        public IEventAggregator EventAggregator { get; set; }

        public bool IsActive { get; private set; }

        public bool IsNotifying { get; set; }

        public bool IsPlaying { get; private set; }

        public Func<IId, Task<PlaylistItem>> LoadModel { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IDialogNotificationService NotificationService { get; set; }

        public int? Parameter { get; set; }

        public ObservableCollection<object> SelectedItems { get; private set; }

        public bool ShuffleOn { get; private set; }

        public Uri Source { get; set; }

        public PlaybackViewModelStateEnum State { get; private set; }

        public ISubsonicService SubsonicService { get; set; }

        public IToastNotificationService ToastNotificationService { get; private set; }

        public Action UpdateDisplayName { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Activate()
        {
            throw new NotImplementedException();
        }

        public void CanClose(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void ClearPlaylist()
        {
            throw new NotImplementedException();
        }

        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StartPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
        }

        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void SavePlaylist()
        {
            throw new NotImplementedException();
        }

        public void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
        }

        public void TryClose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}