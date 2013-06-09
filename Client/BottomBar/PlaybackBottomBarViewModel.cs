namespace Subsonic8.BottomBar
{
    using System.Collections.Specialized;
    using System.Linq;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;
    using Subsonic8.ErrorDialog;

    public class PlaybackBottomBarViewModel : BottomBarViewModelBase, IPlaybackBottomBarViewModel
    {
        #region Constructors and Destructors

        public PlaybackBottomBarViewModel(
            INavigationService navigationService, 
            IEventAggregator eventAggregator, 
            IPlaylistManagementService playlistManagementService, 
            IErrorDialogViewModel errorDialogViewModel)
            : base(navigationService, eventAggregator, playlistManagementService, errorDialogViewModel)
        {
        }

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