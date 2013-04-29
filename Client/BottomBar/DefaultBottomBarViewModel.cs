using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Services;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Action = System.Action;

namespace Subsonic8.BottomBar
{
    public class DefaultBottomBarViewModel : BottomBarViewModelBase, IDefaultBottomBarViewModel
    {
        private readonly ICustomFrameAdapter _navigationService;

        private IEnumerable<ISubsonicModel> SelectedSubsonicItems
        {
            get { return SelectedItems.Cast<IMenuItemViewModel>().Select(vm => vm.Item); }
        }

        public bool CanAddToPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(MenuItemViewModel)); }
        }

        public Action NavigateOnPlay { get; set; }

        public DefaultBottomBarViewModel(ICustomFrameAdapter navigationService, IEventAggregator eventAggregator, IPlaylistManagementService playlistManagementService)
            : base(navigationService, eventAggregator, playlistManagementService)
        {
            _navigationService = navigationService;
            NavigateOnPlay = () => _navigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public void AddToPlaylist()
        {
            EventAggregator.Publish(new PlaylistMessage { Queue = SelectedSubsonicItems.ToList() });
            SelectedItems.Clear();
        }

        public void PlayAll()
        {
            EventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => ((IMenuItemViewModel)i).Item).ToList(), ClearCurrent = true });
            SelectedItems.Clear();
            NavigateOnPlay();
        }

        public void NavigateToPlaylist()
        {
            _navigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        protected override void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanAddToPlaylist);
        }
    }
}
