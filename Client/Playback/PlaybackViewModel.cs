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
using MugenInjection.Attributes;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.VideoPlayback;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : PlaybackControlsViewModelBase, IPlaybackViewModel
    {
        public const string CoverArtPlaceholderLarge = @"/Assets/CoverArtPlaceholderLarge.jpg";

        #region Private Fields

        private IPlaylistManagementService _playlistManagementService;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private string _coverArt;
        private bool _playNextItem;
        private TimeSpan _endTime;
        private TimeSpan _startTime;
        private IEmbededVideoPlaybackViewModel _embededVideoPlaybackViewModel;

        #endregion

        #region Public Properties

        public ISubsonicModel Parameter
        {
            set
            {
                if (value == null) return;
                Handle(new PlayFile { Model = value });
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
                if (value == _state) return;
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

        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                if (value.Equals(_endTime)) return;
                _endTime = value;
                NotifyOfPropertyChange(() => EndTime);
            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                if (value.Equals(_startTime)) return;
                _startTime = value;
                NotifyOfPropertyChange(() => StartTime);
            }
        }

        public PlaylistItemCollection PlaylistItems
        {
            get { return _playlistManagementService.Items; }
        }

        public Func<IId, Task<Client.Common.Models.PlaylistItem>> LoadModel { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        [Inject]
        public IEmbededVideoPlaybackViewModel EmbededVideoPlaybackViewModel
        {
            get
            {
                return _embededVideoPlaybackViewModel;
            }

            set
            {
                if (Equals(value, _embededVideoPlaybackViewModel)) return;
                _embededVideoPlaybackViewModel = value;
                NotifyOfPropertyChange(() => EmbededVideoPlaybackViewModel);
            }
        }

        [Inject]
        public IWinRTWrappersService WinRTWrappersService { get; set; }

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

        #endregion

        public PlaybackViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Playlist";
            LoadModel = LoadModelImpl;
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (Client.Common.Models.PlaylistItem)(((ItemClickEventArgs)e).ClickedItem);
            var pressedItemIndex = PlaylistItems.IndexOf(pressedItem);
            EventAggregator.Publish(new PlayItemAtIndexMessage(pressedItemIndex));
        }

        public void ClearPlaylist()
        {
            _playlistManagementService.Clear();
        }

        public async void LoadPlaylist()
        {
            var storageFile = await WinRTWrappersService.OpenStorageFile();
            if (storageFile != null)
            {
                var playlistItemCollection = await WinRTWrappersService.LoadFromFile<PlaylistItemCollection>(storageFile, PlaylistItems);
                _playlistManagementService.LoadPlaylist(playlistItemCollection);
            }
        }

        public async void SavePlaylist()
        {
            var storageFile = await WinRTWrappersService.GetNewStorageFile();
            if (storageFile != null)
            {
                await WinRTWrappersService.SaveToFile(storageFile, PlaylistItems);
            }
        }

        public void Handle(StartVideoPlaybackMessage message)
        {
            if (!message.FullScreen)
            {
                State = PlaybackViewModelStateEnum.Video;
                if (!IsActive)
                {
                    NavigationService.NavigateToViewModel<PlaybackViewModel>();
                }
            }

            this.ShowToast(message.Item);
        }

        public void Handle(StartAudioPlaybackMessage message)
        {
            CoverArt = message.Item.CoverArtUrl;
            State = PlaybackViewModelStateEnum.Audio;

            this.ShowToast(message.Item);
        }

        public void Handle(PlaylistStateChangedMessage message)
        {
            EventAggregator.Publish(new ShowControlsMessage
                {
                    Show = message.HasElements
                });
        }

        public async void Handle(PlaylistMessage message)
        {
            if (message.ClearCurrent)
            {
                EventAggregator.Publish(new StopPlaybackMessage());
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
            EventAggregator.Publish(new AddItemsMessage { Queue = new List<Client.Common.Models.PlaylistItem>(new[] { playlistItem }) });
            EventAggregator.Publish(new PlayItemAtIndexMessage(PlaylistItems.Count - 1));
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SetupBottomBar();
        }

        private async Task AddItemToPlaylist(ISubsonicModel item)
        {
            if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
            {
                var addItemsMessage = new AddItemsMessage { Queue = new List<Client.Common.Models.PlaylistItem>(new[] { await LoadModel(item) }) };
                EventAggregator.Publish(addItemsMessage);
                if (_playNextItem)
                {
                    _playNextItem = false;
                    EventAggregator.Publish(new PlayNextMessage());
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
                                   .OnSuccess(result => playlistItem = CreatePlaylistItemFromSong(result)).Execute();
            }

            return playlistItem;
        }

        private void SetupBottomBar()
        {
            BottomBar.IsOpened = false;
            BottomBar.IsOnPlaylist = true;
        }

        private Client.Common.Models.PlaylistItem CreatePlaylistItemFromSong(Song result)
        {
            var playlistItem = new Client.Common.Models.PlaylistItem();
            playlistItem.InitializeFromSong(result, SubsonicService);

            return playlistItem;
        }

        private void HookPlaylistManagementService()
        {
            PlaylistManagementService.PropertyChanged += PlaylistManagementServiceOnPropertyChanged;
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
    }
}