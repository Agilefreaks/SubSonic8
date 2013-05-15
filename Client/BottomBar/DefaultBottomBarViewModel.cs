using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;
using Subsonic8.MenuItem;
using Subsonic8.Playback;
using Action = System.Action;

namespace Subsonic8.BottomBar
{
    public class DefaultBottomBarViewModel : BottomBarViewModelBase, IDefaultBottomBarViewModel
    {
        private bool _playNextItem;

        public bool CanAddToPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(MenuItemViewModel)); }
        }

        public Action NavigateOnPlay { get; set; }

        public Func<IId, Task<Client.Common.Models.PlaylistItem>> LoadModel { get; set; }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        private IEnumerable<ISubsonicModel> SelectedSubsonicItems
        {
            get { return SelectedItems.Cast<IMenuItemViewModel>().Select(vm => vm.Item); }
        }

        public DefaultBottomBarViewModel(ICustomFrameAdapter navigationService, IEventAggregator eventAggregator,
            IPlaylistManagementService playlistManagementService)
            : base(navigationService, eventAggregator, playlistManagementService)
        {
            LoadModel = this.LoadSong;
            NavigateOnPlay = () => NavigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public void AddToPlaylist()
        {
            AddToPlaylist(SelectedSubsonicItems.ToList());
            SelectedItems.Clear();
        }

        public void PlayAll()
        {
            AddToPlaylist(SelectedSubsonicItems.ToList(), true);
            SelectedItems.Clear();
            NavigateOnPlay();
        }

        public void NavigateToPlaylist()
        {
            NavigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public async void HandleError(Exception error)
        {
            await NotificationService.Show(new DialogNotificationOptions
                {
                    Message = error.ToString(),
                });
        }

        protected override void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanAddToPlaylist);
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

        private async void AddToPlaylist(IEnumerable<ISubsonicModel> items, bool clearCurrent = false)
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
    }
}
