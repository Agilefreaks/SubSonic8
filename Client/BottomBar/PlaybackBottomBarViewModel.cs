namespace Subsonic8.BottomBar
{
    using System.Collections.Specialized;
    using System.Linq;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;

    public class PlaybackBottomBarViewModel : BottomBarViewModelBase, IPlaybackBottomBarViewModel
    {
        #region Constructors and Destructors

        #endregion

        #region Public Properties

        public bool CanRemoveFromPlaylist
        {
            get
            {
                return SelectedItems.Any();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void RemoveFromPlaylist()
        {
            EventAggregator.Publish(
                new RemoveItemsMessage { Queue = SelectedItems.Select(x => (PlaylistItem)x).ToList() });
        }

        #endregion

        #region Methods

        protected override void OnSelectedItemsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanRemoveFromPlaylist);
        }

        #endregion
    }
}