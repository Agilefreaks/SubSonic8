using System;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Services;
using Subsonic8.MenuItem;

namespace Subsonic8.BottomBar
{
    public class PlaylistBottomBarViewModel : BottomBarViewModelBase, IPlaylistBottomBarViewModel
    {
        public bool CanDeletePlaylist
        {
            get { return SelectedItems.Any(); }
        }

        public System.Action OnPlaylistDeleted { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        public PlaylistBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IPlaylistManagementService playlistManagementService)
            : base(navigationService, eventAggregator, playlistManagementService)
        {
        }

        public async void DeletePlaylist()
        {
            var playlistId = ((MenuItemViewModel)SelectedItems[0]).Item.Id;
            await SubsonicService.DeletePlaylist(playlistId)
                                 .WithErrorHandler(this)
                                 .OnSuccess(result => OnPlaylistDeleted())
                                 .Execute();
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
            NotifyOfPropertyChange(() => CanDeletePlaylist);
        }
    }
}