namespace Subsonic8.BottomBar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.MenuItem;
    using Subsonic8.Playback;
    using Action = System.Action;

    public class DefaultBottomBarViewModel : BottomBarViewModelBase, IDefaultBottomBarViewModel
    {
        #region Fields

        private bool _playNextItem;

        #endregion

        #region Constructors and Destructors

        public DefaultBottomBarViewModel()
        {
            LoadPlaylistItem = this.LoadPlaylistItemFromSong;
            NavigateOnPlay = () => NavigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        #endregion

        #region Public Properties

        public bool CanAddToPlaylist
        {
            get
            {
                return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(MenuItemViewModel));
            }
        }

        public Func<IId, Task<PlaylistItem>> LoadPlaylistItem { get; set; }

        public Action NavigateOnPlay { get; set; }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        #endregion

        #region Explicit Interface Properties

        IErrorHandler ISongLoader.ErrorHandler
        {
            get
            {
                return ErrorDialogViewModel;
            }
        }

        #endregion

        #region Properties

        private IEnumerable<ISubsonicModel> SelectedSubsonicItems
        {
            get
            {
                return SelectedItems.Select(vm => ((IMenuItemViewModel)vm).Item);
            }
        }

        #endregion

        #region Public Methods and Operators

        public async Task AddToPlaylist()
        {
            await AddToPlaylist(SelectedSubsonicItems.ToList());
            SelectedItems.Clear();
        }

        public void NavigateToPlaylist()
        {
            CanDismiss = true;
            IsOpened = false;
            NavigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public async Task PlayAll()
        {
            await AddToPlaylist(SelectedSubsonicItems.ToList(), true);
            SelectedItems.Clear();
            NavigateOnPlay();
        }

        #endregion

        #region Methods

        protected override void OnSelectedItemsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanAddToPlaylist);
        }

        private async Task AddItemToPlaylist(ISubsonicModel item)
        {
            if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
            {
                var playlistItems = new List<PlaylistItem>(new[] { await LoadPlaylistItem(item) });
                var addItemsMessage = new AddItemsMessage { Queue = playlistItems };
                if (_playNextItem)
                {
                    _playNextItem = false;
                    addItemsMessage.StartPlaying = true;
                }

                EventAggregator.Publish(addItemsMessage);
            }
            else
            {
                var children = new List<ISubsonicModel>();
                switch (item.Type)
                {
                    case SubsonicModelTypeEnum.Album:
                        {
                            await
                                SubsonicService.GetAlbum(item.Id)
                                               .WithErrorHandler(ErrorDialogViewModel)
                                               .OnSuccess(result => children.AddRange(result.Songs))
                                               .Execute();
                        }

                        break;
                    case SubsonicModelTypeEnum.Artist:
                        {
                            await
                                SubsonicService.GetArtist(item.Id)
                                               .WithErrorHandler(ErrorDialogViewModel)
                                               .OnSuccess(result => children.AddRange(result.Albums))
                                               .Execute();
                        }

                        break;
                    case SubsonicModelTypeEnum.MusicDirectory:
                        {
                            await
                                SubsonicService.GetMusicDirectory(item.Id)
                                               .WithErrorHandler(ErrorDialogViewModel)
                                               .OnSuccess(result => children.AddRange(result.Children))
                                               .Execute();
                        }

                        break;
                    case SubsonicModelTypeEnum.Index:
                        {
                            children.AddRange(((IndexItem)item).Artists);
                        }

                        break;
                    case SubsonicModelTypeEnum.Folder:
                        {
                            await
                                SubsonicService.GetIndex(item.Id)
                                               .WithErrorHandler(ErrorDialogViewModel)
                                               .OnSuccess(result => children.AddRange(result.Artists))
                                               .Execute();
                        }

                        break;
                }

                foreach (var subsonicModel in children)
                {
                    await AddItemToPlaylist(subsonicModel);
                }
            }
        }

        private async Task AddToPlaylist(IEnumerable<ISubsonicModel> items, bool clearCurrent = false)
        {
            if (clearCurrent)
            {
                EventAggregator.Publish(new StopMessage());
                PlaylistManagementService.Clear();
                _playNextItem = true;
            }

            foreach (var item in items)
            {
                await AddItemToPlaylist(item);
            }
        }

        #endregion
    }
}