namespace Subsonic8.Playback
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Client.Common;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services;
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

        private const string StatePlaylistKey = "playlist_items";

        #endregion

        #region Fields

        private IPlaybackBottomBarViewModel _bottomBar;

        private string _coverArt;

        private IEmbededVideoPlaybackViewModel _embededVideoPlaybackViewModel;

        private IFullScreenVideoPlaybackViewModel _fullScreenVideoPlaybackViewModel;

        private bool _playbackControlsVisible;

        private IPlayerManagementService _playerManagementService;

        private IPlaylistManagementService _playlistManagementService;

        private Uri _source;

        private PlaybackViewModelStateEnum _state;

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

        public object ActiveItem
        {
            get
            {
                return PlaylistManagementService.CurrentItem;
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
                if (Equals(value, _fullScreenVideoPlaybackViewModel))
                {
                    return;
                }

                _fullScreenVideoPlaybackViewModel = value;
                NotifyOfPropertyChange();
                HookFullScreenVideoPlaybackViewModel();
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

        public PlaylistItemCollection PlaylistItems
        {
            get
            {
                return _playlistManagementService.Items;
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

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return BottomBar != null ? BottomBar.SelectedItems : new ObservableCollection<object>();
            }
        }

        public Uri Source
        {
            get
            {
                return _source;
            }

            set
            {
                try
                {
                    _source = value;
                    NotifyOfPropertyChange();
                }
                catch (Exception exception)
                {
                    // This is due to a bug in winrt sdk
                    Debug.WriteLine(exception.ToString());
                }
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
        public ITileNotificationService TileNotificationService { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        [Inject]
        public IWinRTWrappersService WinRTWrappersService { get; set; }

        #endregion

        #region Public Methods and Operators

        public async Task AddToPlaylistAndPlay(Song model)
        {
            var playlistItem = await LoadModel(model);
            PlaylistManagementService.Items.Add(playlistItem);
            EventAggregator.Publish(new PlayItemAtIndexMessage(PlaylistItems.Count - 1));
        }

        public void ClearPlaylist()
        {
            _playlistManagementService.Clear();
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
            if (!statePageState.ContainsKey(StatePlaylistKey) || PlaylistItems.Any())
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

            PlaylistItems.Clear();
            PlaylistItems.AddRange(playlist);
        }

        public async void SavePlaylist()
        {
            var storageFile = await WinRTWrappersService.GetNewStorageFile();
            if (storageFile != null)
            {
                await WinRTWrappersService.SaveToFile(storageFile, PlaylistItems);
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
                xmlSerializer.Serialize(memoryStream, PlaylistItems);
                memoryStream.Flush();
                statePageState.Add(StatePlaylistKey, Convert.ToBase64String(memoryStream.ToArray()));
            }
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (PlaylistItem)((ItemClickEventArgs)e).ClickedItem;
            var pressedItemIndex = PlaylistItems.IndexOf(pressedItem);
            EventAggregator.Publish(new PlayItemAtIndexMessage(pressedItemIndex));
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
                (sender, eventArgs) => SwitchToFullScreenVideoPlayback(eventArgs);
        }

        private void HookFullScreenVideoPlaybackViewModel()
        {
            FullScreenVideoPlaybackViewModel.FullScreenChanged +=
                (sender, eventArgs) => SwitchToEmbededVideoPlayback(eventArgs);
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
                               .WithErrorHandler(this)
                               .OnSuccess(async song => await AddToPlaylistAndPlay(song))
                               .Execute();
        }

        private void PlaylistManagementServiceOnPropertyChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName
                == _playlistManagementService.GetPropertyName(() => _playlistManagementService.IsPlaying))
            {
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        private void SetAppBottomBar()
        {
            EventAggregator.Publish(new ChangeBottomBarMessage { BottomBarViewModel = BottomBar });
        }

        private void SetStateByCurrentPlayer()
        {
            State = PlayerManagementService.CurrentPlayer == EmbededVideoPlaybackViewModel
                        ? PlaybackViewModelStateEnum.Video
                        : PlayerManagementService.CurrentPlayer == FullScreenVideoPlaybackViewModel
                              ? PlaybackViewModelStateEnum.FullScreen
                              : PlaybackViewModelStateEnum.Audio;
        }

        private void SwitchToEmbededVideoPlayback(PlaybackStateEventArgs eventArgs)
        {
            EventAggregator.Publish(new StopMessage());
            PlayerManagementService.DefaultVideoPlayer = EmbededVideoPlaybackViewModel;
            EventAggregator.Publish(new PlayMessage { Options = eventArgs });
        }

        private void SwitchToFullScreenVideoPlayback(PlaybackStateEventArgs eventArgs)
        {
            EventAggregator.Publish(new StopMessage());
            PlayerManagementService.DefaultVideoPlayer = FullScreenVideoPlaybackViewModel;
            EventAggregator.Publish(new PlayMessage { Options = eventArgs });
        }

        #endregion
    }
}