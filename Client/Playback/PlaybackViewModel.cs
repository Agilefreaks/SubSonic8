namespace Subsonic8.Playback
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using global::Common.ListCollectionView;
    using MugenInjection.Attributes;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.Playlists;
    using Subsonic8.VideoPlayback;
    using Windows.UI.Xaml.Controls;

    public class PlaybackViewModel : PlaybackControlsViewModelBase, IPlaybackViewModel
    {
        #region Constants

        public const string CoverArtPlaceholderLarge = @"/Assets/CoverArtPlaceholderLarge.jpg";

        public const string SnappedStateName = "Snapped";

        private const string StatePlaylistKey = "playlist_items";

        #endregion

        #region Fields

        private IPlaybackBottomBarViewModel _bottomBar;

        private string _coverArt;

        private IEmbededVideoPlaybackViewModel _embededVideoPlaybackViewModel;

        private string _filterText;

        private IFullScreenVideoPlaybackViewModel _fullScreenVideoPlaybackViewModel;

        private bool _isFiltering;

        private bool _playbackControlsVisible;

        private IPlayerManagementService _playerManagementService;

        private ListCollectionView _playlistItems;

        private IPlaylistManagementService _playlistManagementService;

        private PlaybackViewModelStateEnum _previousState;

        private PlaybackViewModelStateEnum _state;

        private ISnappedVideoPlaybackViewModel _snappedVideoPlaybackViewModel;

        private string _currentVisualState;

        #endregion

        #region Constructors and Destructors

        public PlaybackViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Playlist";
            CoverArt = CoverArtPlaceholderLarge;
            LoadModel = this.LoadSong;
        }

        #endregion

        #region Public Properties

        public PlaylistItem ActiveItem
        {
            get
            {
                return PlaylistManagementService.CurrentItem;
            }
        }

        public string CoverArt
        {
            get
            {
                return _coverArt;
            }

            set
            {
                _coverArt = value == Client.Common.Services.SubsonicService.CoverArtPlaceholder
                                ? CoverArtPlaceholderLarge
                                : value;
                NotifyOfPropertyChange();
            }
        }

        public string FilterText
        {
            get
            {
                return _filterText;
            }

            set
            {
                if (value == _filterText)
                {
                    return;
                }

                _filterText = value;
                NotifyOfPropertyChange();
                SetPlaylistFilter(_filterText);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _playlistManagementService.IsPlaying;
            }
        }

        public Func<IId, Task<PlaylistItem>> LoadModel { get; set; }

        public int? Parameter
        {
            set
            {
                if (value.HasValue)
                {
                    LoadSongById(value.Value);
                }
            }
        }

        public bool PlaybackControlsVisible
        {
            get
            {
                return _playbackControlsVisible;
            }

            set
            {
                if (value.Equals(_playbackControlsVisible))
                {
                    return;
                }

                _playbackControlsVisible = value;
                NotifyOfPropertyChange();
            }
        }

        public ListCollectionView PlaylistItems
        {
            get
            {
                return _playlistItems ?? (_playlistItems = new ListCollectionView(_playlistManagementService.Items));
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return BottomBar != null ? BottomBar.SelectedItems : new ObservableCollection<object>();
            }
        }

        public PlaybackViewModelStateEnum State
        {
            get
            {
                return _state;
            }

            set
            {
                if (value == _state)
                {
                    return;
                }

                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        [Inject]
        public IPlaybackBottomBarViewModel BottomBar
        {
            get
            {
                return _bottomBar;
            }

            set
            {
                if (Equals(value, _bottomBar))
                {
                    return;
                }

                _bottomBar = value;
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public IEmbededVideoPlaybackViewModel EmbededVideoPlaybackViewModel
        {
            get
            {
                return _embededVideoPlaybackViewModel;
            }

            set
            {
                if (Equals(value, _embededVideoPlaybackViewModel))
                {
                    return;
                }

                _embededVideoPlaybackViewModel = value;
                NotifyOfPropertyChange(() => EmbededVideoPlaybackViewModel);
                HookEmbededVideoPlaybackViewModel();
            }
        }

        [Inject]
        public IFullScreenVideoPlaybackViewModel FullScreenVideoPlaybackViewModel
        {
            get
            {
                return _fullScreenVideoPlaybackViewModel;
            }

            set
            {
                if (Equals(value, _fullScreenVideoPlaybackViewModel)) return;
                _fullScreenVideoPlaybackViewModel = value;
                NotifyOfPropertyChange();
                HookFullScreenVideoPlaybackViewModel();
            }
        }

        [Inject]
        public ISnappedVideoPlaybackViewModel SnappedVideoPlaybackViewModel
        {
            get
            {
                return _snappedVideoPlaybackViewModel;
            }

            set
            {
                if (Equals(value, _snappedVideoPlaybackViewModel)) return;
                _snappedVideoPlaybackViewModel = value;
                NotifyOfPropertyChange();
                HookSnapeedVideoPlaybackViewModel();
            }
        }

        [Inject]
        public IPlayerManagementService PlayerManagementService
        {
            get
            {
                return _playerManagementService;
            }

            set
            {
                if (Equals(value, _playerManagementService))
                {
                    return;
                }

                _playerManagementService = value;
                HookPlayerManagementService();
            }
        }

        [Inject]
        public IPlaylistManagementService PlaylistManagementService
        {
            get
            {
                return _playlistManagementService;
            }

            set
            {
                _playlistManagementService = value;
                HookPlaylistManagementService();
            }
        }

        [Inject]
        public ITileNotificationService TileNotificationService { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        [Inject]
        public IWinRTWrappersService WinRTWrappersService { get; set; }

        #endregion

        #region Properties

        private bool IsFiltering
        {
            get
            {
                return _isFiltering;
            }

            set
            {
                _isFiltering = value;
                OnIsFilteringChanged();
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        public async Task AddToPlaylistAndPlay(Song model)
        {
            var playlistItem = await LoadModel(model);
            PlaylistManagementService.Items.Add(playlistItem);
            EventAggregator.Publish(new PlayItemAtIndexMessage(PlaylistManagementService.Items.Count - 1));
        }

        public void ClearPlaylist()
        {
            _playlistManagementService.Clear();
        }

        public void DoneFiltering()
        {
            IsFiltering = false;
        }

        public void Handle(StartPlaybackMessage message)
        {
            CoverArt = message.Item.OriginalCoverArtUrl;
            NotifyOfPropertyChange(() => ActiveItem);
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            PlaybackControlsVisible = message.HasElements;
        }

        public void Handle(PlayFailedMessage message)
        {
            EventAggregator.Publish(new StopMessage());
            NotificationService.Show(new DialogNotificationOptions
                                         {
                                             Message = "Could not play item:\r\n" + message.ErrorMessage
                                         });
        }

        public async void LoadPlaylist()
        {
            var storageFile = await WinRTWrappersService.OpenStorageFile();
            if (storageFile == null)
            {
                return;
            }

            var playlistItemCollection = await WinRTWrappersService.LoadFromFile<PlaylistItemCollection>(storageFile);
            _playlistManagementService.LoadPlaylist(playlistItemCollection);
        }

        public void LoadRemotePlaylist()
        {
            NavigationService.NavigateToViewModel<ManagePlaylistsViewModel>();
        }

        public void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
            if (!statePageState.ContainsKey(StatePlaylistKey) || PlaylistManagementService.Items.Any())
            {
                return;
            }

            var bytes = Convert.FromBase64String((string)statePageState[StatePlaylistKey]);
            PlaylistItemCollection playlist;
            using (var memoryStream = new MemoryStream(bytes))
            {
                var xmlSerializer = new XmlSerializer(typeof(PlaylistItemCollection));
                playlist = (PlaylistItemCollection)xmlSerializer.Deserialize(memoryStream);
            }

            PlaylistManagementService.Items.Clear();
            PlaylistManagementService.Items.AddRange(playlist);
        }

        public async void SavePlaylist()
        {
            var storageFile = await WinRTWrappersService.GetNewStorageFile();
            if (storageFile != null)
            {
                await WinRTWrappersService.SaveToFile(storageFile, PlaylistManagementService.Items);
            }
        }

        public void SaveRemotePlaylist()
        {
            NavigationService.NavigateToViewModel<SavePlaylistViewModel>();
        }

        public void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
            knownTypes.Add(typeof(string));
            var xmlSerializer = new XmlSerializer(typeof(PlaylistItemCollection));
            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, PlaylistManagementService.Items);
                memoryStream.Flush();
                statePageState.Add(StatePlaylistKey, Convert.ToBase64String(memoryStream.ToArray()));
            }
        }

        public void ShowFilter()
        {
            if (IsFiltering)
            {
                return;
            }

            IsFiltering = true;
        }

        public void StartPlayback(object e)
        {
            if (State == PlaybackViewModelStateEnum.Filter)
            {
                IsFiltering = false;
            }

            var pressedItem = (PlaylistItem)((ItemClickEventArgs)e).ClickedItem;
            var pressedItemIndex = PlaylistManagementService.Items.IndexOf(pressedItem);
            EventAggregator.Publish(new PlayItemAtIndexMessage(pressedItemIndex));
        }

        public void OnVisualStateChanged(string state)
        {
            if (_currentVisualState == state) return;

            if (_currentVisualState == SnappedStateName && (State == PlaybackViewModelStateEnum.FullScreen || State == PlaybackViewModelStateEnum.Video))
            {
                var playbackTimeInfo = ((IVideoPlayer)PlayerManagementService.CurrentPlayer).GetPlaybackTimeInfo();
                var videoPlayer = State == PlaybackViewModelStateEnum.FullScreen
                                      ? (IVideoPlayer)FullScreenVideoPlaybackViewModel
                                      : EmbededVideoPlaybackViewModel;
                SwitchVideoPlayback(playbackTimeInfo, videoPlayer);
            }
            else if (state == SnappedStateName && (State == PlaybackViewModelStateEnum.FullScreen || State == PlaybackViewModelStateEnum.Video))
            {
                var playbackTimeInfo = ((IVideoPlayer)PlayerManagementService.CurrentPlayer).GetPlaybackTimeInfo();
                SwitchVideoPlayback(playbackTimeInfo, SnappedVideoPlaybackViewModel);
            }

            _currentVisualState = state;
        }

        #endregion

        #region Methods

        protected override void OnActivate()
        {
            base.OnActivate();
            SetAppBottomBar();
        }

        private void HookEmbededVideoPlaybackViewModel()
        {
            EmbededVideoPlaybackViewModel.FullScreenChanged +=
                (sender, eventArgs) => SwitchVideoPlayback(eventArgs, FullScreenVideoPlaybackViewModel);
        }

        private void HookFullScreenVideoPlaybackViewModel()
        {
            FullScreenVideoPlaybackViewModel.FullScreenChanged +=
                (sender, eventArgs) => SwitchVideoPlayback(eventArgs, EmbededVideoPlaybackViewModel);
        }

        private void HookSnapeedVideoPlaybackViewModel()
        {
            SnappedVideoPlaybackViewModel.FullScreenChanged +=
                (sender, eventArgs) => SwitchVideoPlayback(eventArgs, FullScreenVideoPlaybackViewModel);
        }

        private void HookPlayerManagementService()
        {
            PlayerManagementService.CurrentPlayerChanged += (sender, eventArgs) => SetStateByCurrentPlayer();
        }

        private void HookPlaylistManagementService()
        {
            PlaylistManagementService.PropertyChanged += PlaylistManagementServiceOnPropertyChanged;
        }

        private async void LoadSongById(int songId)
        {
            await
                SubsonicService.GetSong(songId)
                               .WithErrorHandler(ErrorDialogViewModel)
                               .OnSuccess(async song => await AddToPlaylistAndPlay(song))
                               .Execute();
        }

        private void OnIsFilteringChanged()
        {
            if (IsFiltering)
            {
                _previousState = State;
                State = PlaybackViewModelStateEnum.Filter;
            }
            else
            {
                FilterText = string.Empty;
                State = _previousState;
            }
        }

        private void PlaylistManagementServiceOnPropertyChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName
                == global::Common.ExtensionsMethods.ObjectExtensionMethods.GetPropertyName(
                    _playlistManagementService, () => _playlistManagementService.IsPlaying))
            {
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        private void SetAppBottomBar()
        {
            EventAggregator.Publish(new ChangeBottomBarMessage { BottomBarViewModel = BottomBar });
        }

        private void SetPlaylistFilter(string filterText)
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                PlaylistItems.Filter = null;
            }
            else
            {
                filterText = filterText.ToLower();
                Func<string, string> prepareValueForFilter =
                    value => value == null ? string.Empty : value.ToLowerInvariant();
                PlaylistItems.Filter = element =>
                    {
                        var playlistItem = (PlaylistItem)element;
                        return prepareValueForFilter(playlistItem.Artist).Contains(filterText)
                               || prepareValueForFilter(playlistItem.Title).Contains(filterText);
                    };
            }
        }

        private void SetStateByCurrentPlayer()
        {
            if (IsFiltering) return;

            var currentPlayer = PlayerManagementService.CurrentPlayer;
            State = currentPlayer == EmbededVideoPlaybackViewModel
                        ? PlaybackViewModelStateEnum.Video
                        : currentPlayer == FullScreenVideoPlaybackViewModel
                              ? PlaybackViewModelStateEnum.FullScreen
                              : currentPlayer == SnappedVideoPlaybackViewModel
                                    ? State
                                    : PlaybackViewModelStateEnum.Audio;
        }

        private void SwitchVideoPlayback(PlaybackStateEventArgs eventArgs, IVideoPlayer videoPlayer)
        {
            EventAggregator.Publish(new StopMessage());
            PlayerManagementService.DefaultVideoPlayer = videoPlayer;
            EventAggregator.Publish(new PlayMessage { Options = eventArgs });
        }

        #endregion
    }
}