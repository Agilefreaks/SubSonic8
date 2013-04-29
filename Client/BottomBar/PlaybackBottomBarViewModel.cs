using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Services;

namespace Subsonic8.BottomBar
{
    public class PlaybackBottomBarViewModel : BottomBarViewModelBase, IPlaybackBottomBarViewModel
    {
        public bool CanRemoveFromPlaylist
        {
            get { return SelectedItems.Any(); }
        }

        public PlaybackBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IPlaylistManagementService playlistManagementService)
            : base(navigationService, eventAggregator, playlistManagementService)
        {
        }

        public void RemoveFromPlaylist()
        {
            EventAggregator.Publish(new RemoveItemsMessage { Queue = SelectedItems.Select(x => (Client.Common.Models.PlaylistItem)x).ToList() });
        }

        protected override void OnSelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanRemoveFromPlaylist);
        }
    }
}