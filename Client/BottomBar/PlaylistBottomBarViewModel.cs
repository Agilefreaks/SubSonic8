using System;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.MenuItem;

namespace Subsonic8.BottomBar
{
    public class PlaylistBottomBarViewModel : BottomBarViewModelBase, IPlaylistBottomBarViewModel
    {
        public bool CanDeletePlaylist
        {
            get { return SelectedItems.Any(); }
        }

        public Action<int> DeletePlaylistAction { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        public PlaylistBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IPlaylistManagementService playlistManagementService)
            : base(navigationService, eventAggregator, playlistManagementService)
        {
        }

        public void DeletePlaylist()
        {
            DeletePlaylistAction(((MenuItemViewModel) SelectedItems[0]).Item.Id);
        }

        protected override void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanDeletePlaylist);
        }
    }
}