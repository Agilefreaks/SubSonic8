using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.Shell;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : PlaybackControlsViewModelBase, IPlaybackViewModel
    {
        public const string CoverArtPlaceholderLarge = @"/Assets/CoverArtPlaceholderLarge.jpg";

        #region Private Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly IToastNotificationService _notificationService;
        private readonly IWinRTWrappersService _winRTWrappersService;
        private readonly IPlaylistManagementService _playlistManagementService;
        private IShellViewModel _shellViewModel;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private string _coverArt;
        private bool _playNextItem;

        #endregion

        #region Public Properties

        public IShellViewModel ShellViewModel
        {
            get
            {
                return _shellViewModel;
            }

            set
            {
                _shellViewModel = value;
                NotifyOfPropertyChange();
            }
        }

        public ISubsonicModel Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (value == _parameter) return;
                _parameter = value;
                if (_parameter != null)
                {
                    Handle(new PlayFile { Model = _parameter });
                }
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
                    //This is due to a bug in winrt sdk
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
                if (_state == value) return;

                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        public bool IsPlaying
        {
            get { return _playlistManagementService.IsPlaying; }
        }

        public bool ShuffleOn
        {
            get { return _playlistManagementService.ShuffleOn; }
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

        public PlaylistItemCollection PlaylistItems
        {
            get { return _playlistManagementService.Items; }
        }

        public Func<IId, Task<Client.Common.Models.PlaylistItem>> LoadModel { get; set; }

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel,
            IToastNotificationService notificationService, IWinRTWrappersService winRTWrappersService,
            IPlaylistManagementService playlistManagementService)
            : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _winRTWrappersService = winRTWrappersService;
            _eventAggregator.Subscribe(this);
            _playlistManagementService = playlistManagementService;

            ShellViewModel = shellViewModel;
            State = PlaybackViewModelStateEnum.Empty;

            UpdateDisplayName = () => DisplayName = "Playlist";
            LoadModel = LoadModelImpl;

            HookPlaylistManagementService();
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (Client.Common.Models.PlaylistItem)(((ItemClickEventArgs)e).ClickedItem);
            var pressedItemIndex = PlaylistItems.IndexOf(pressedItem);
            _eventAggregator.Publish(new PlayItemAtIndexMessage(pressedItemIndex));
        }

        public void ClearPlaylist()
        {
            _playlistManagementService.Clear();
        }

        public async void LoadPlaylist()
        {
            var storageFile = await _winRTWrappersService.OpenStorageFile();
            if (storageFile != null)
            {
                var playlistItemCollection = await _winRTWrappersService.LoadFromFile<PlaylistItemCollection>(storageFile, PlaylistItems);
                _playlistManagementService.LoadPlaylist(playlistItemCollection);
            }
        }

        public async void SavePlaylist()
        {
            var storageFile = await _winRTWrappersService.GetNewStorageFile();
            if (storageFile != null)
            {
                await _winRTWrappersService.SaveToFile(storageFile, PlaylistItems);
            }
        }

        public void Handle(StartVideoPlaybackMessage message)
        {
            State = PlaybackViewModelStateEnum.Video;
            Source = message.Item.Uri;
            ShowToast(message.Item);
        }

        public void Handle(StartAudioPlaybackMessage message)
        {
            CoverArt = message.Item.CoverArtUrl;
            State = PlaybackViewModelStateEnum.Audio;
            ShowToast(message.Item);
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            _eventAggregator.Publish(new ShowControlsMessage
                {
                    Show = message.HasElements
                });
        }

        public void Handle(StopVideoPlaybackMessage message)
        {
            Source = null;
        }

        public async void Handle(PlaylistMessage message)
        {
            if (message.ClearCurrent)
            {
                _eventAggregator.Publish(new StopPlaybackMessage());
                _playlistManagementService.Clear();
                _playNextItem = true;
            }

            foreach (var item in message.Queue)
            {
                await AddItemToPlaylist(item);
            }
        }

        public async void Handle(PlayFile message)
        {
            var playlistItem = await LoadModel(message.Model);
            _eventAggregator.Publish(new AddItemsMessage { Queue = new List<Client.Common.Models.PlaylistItem>(new[] { playlistItem }) });
            _eventAggregator.Publish(new PlayItemAtIndexMessage(PlaylistItems.Count - 1));
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var oldState = _state;
            State = PlaybackViewModelStateEnum.Empty;
            State = oldState;
            BottomBar.IsOpened = false;
            BottomBar.IsOnPlaylist = true;
        }

        private async Task AddItemToPlaylist(ISubsonicModel item)
        {
            if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
            {
                var addItemsMessage = new AddItemsMessage { Queue = new List<Client.Common.Models.PlaylistItem>(new[] { await LoadModel(item) }) };
                _eventAggregator.Publish(addItemsMessage);
                if (_playNextItem)
                {
                    _playNextItem = false;
                    _eventAggregator.Publish(new PlayNextMessage());
                }
            }
            else
            {
                var children = new List<ISubsonicModel>();
                switch (item.Type)
                {
                    case SubsonicModelTypeEnum.Album:
                        {
                            await SubsonicService.GetAlbum(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Songs))
                                                 .Execute();

                        } break;
                    case SubsonicModelTypeEnum.Artist:
                        {
                            await SubsonicService.GetArtist(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Albums))
                                                 .Execute();
                        } break;
                    case SubsonicModelTypeEnum.MusicDirectory:
                        {
                            await SubsonicService.GetMusicDirectory(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Children))
                                                 .Execute();
                        } break;
                    case SubsonicModelTypeEnum.Index:
                        {
                            children.AddRange(((IndexItem)item).Artists);
                        } break;
                }

                foreach (var subsonicModel in children)
                {
                    await AddItemToPlaylist(subsonicModel);
                }
            }
        }

        private async Task<Client.Common.Models.PlaylistItem> LoadModelImpl(IId model)
        {
            Client.Common.Models.PlaylistItem playlistItem = null;
            if (model != null)
            {
                await
                    SubsonicService.GetSong(model.Id)
                                   .WithErrorHandler(this)
                                   .OnSuccess(result => playlistItem = new Client.Common.Models.PlaylistItem
                                       {
                                           Artist = result.Artist,
                                           Title = result.Title,
                                           Uri = SubsonicService.GetUriForFileWithId(result.Id),
                                           CoverArtUrl = SubsonicService.GetCoverArtForId(result.CoverArt),
                                           PlayingState = PlaylistItemState.NotPlaying,
                                           Duration = result.Duration,
                                           Type = result.Type == SubsonicModelTypeEnum.Video
                                                   ? PlaylistItemTypeEnum.Video
                                                   : PlaylistItemTypeEnum.Audio
                                       }).Execute();
            }

            return playlistItem;
        }

        private void HookPlaylistManagementService()
        {
            _playlistManagementService.PropertyChanged += PlaylistManagementServiceOnPropertyChanged;
        }

        private void PlaylistManagementServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == _playlistManagementService.GetPropertyName(() => _playlistManagementService.ShuffleOn))
            {
                NotifyOfPropertyChange(() => ShuffleOn);
            }

            if (propertyChangedEventArgs.PropertyName == _playlistManagementService.GetPropertyName(() => _playlistManagementService.IsPlaying))
            {
                BottomBar.IsPlaying = _playlistManagementService.IsPlaying;
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        private void ShowToast(Client.Common.Models.PlaylistItem model)
        {
            _notificationService.Show(new ToastNotificationOptions
                {
                    ImageUrl = model.CoverArtUrl,
                    Title = model.Title,
                    Subtitle = model.Artist
                });
        }
    }
}