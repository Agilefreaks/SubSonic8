﻿namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Caliburn.Micro;
    using Client.Common.Results;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Subsonic8.BottomBar;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework.Services;
    using Subsonic8.VideoPlayback;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.UI.Xaml;
    using global::Common.Interfaces;
    using Action = System.Action;
    using PlaylistItem = Client.Common.Models.PlaylistItem;

    public class MockEmbeddedVideoPlaybackViewModel : IEmbeddedVideoPlaybackViewModel
    {
        #region Public Events

        public event EventHandler<ActivationEventArgs> Activated;

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public event EventHandler<PlaybackStateEventArgs> FullScreenChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public IDefaultBottomBarViewModel BottomBar { get; set; }

        public bool CanGoBack { get; private set; }

        public string DisplayName { get; set; }

        public TimeSpan EndTime { get; set; }

        public IErrorHandler ErrorHandler { get; private set; }

        public IEventAggregator EventAggregator { get; set; }

        public bool IsActive { get; private set; }

        public bool IsNotifying { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IDialogNotificationService NotificationService { get; set; }

        public ObservableCollection<object> SelectedItems { get; private set; }

        public TimeSpan StartTime { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        public IToastNotificationService ToastNotificationService { get; private set; }

        public Action UpdateDisplayName { get; set; }

        public IErrorDialogViewModel ErrorDialogViewModel { get; set; }

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

        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void OnFullScreenChanged()
        {
        }

        public void OnShareRequested(DataRequest dataRequest)
        {
        }

        public void Pause()
        {
        }

        public void Play(PlaylistItem item, object options = null)
        {
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
        }

        public void ClearSource()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public PlaybackStateEventArgs GetPlaybackTimeInfo()
        {
            throw new NotImplementedException();
        }

        public void TryClose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}