using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Subsonic8.PlaylistItem;

namespace Subsonic8.BottomBar
{
    public class DefaultBottomBarViewModel : Screen, IDefaultBottomBarViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private bool _isOpened;
        private ObservableCollection<object> _selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return _selectedItems;
            }

            set
            {
                if (_selectedItems == value) return;
                ManageSelectedItemsHooks(value, _selectedItems);
                _selectedItems = value;
                UpdateIsOpened();
                NotifyOfPropertyChange();
            }
        }

        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }

            set
            {
                _isOpened = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanAddToPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(MenuItemViewModel)); } 
        }

        public bool CanRemoveFromPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof (PlaylistItemViewModel)); }
        }

        public DefaultBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            SelectedItems = new ObservableCollection<object>();
            SelectedItems.CollectionChanged += OnSelectedItemsChanged;
        }

        public void AddToPlaylist()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => ((IMenuItemViewModel)i).Item).ToList() });
            SelectedItems.Clear();
        }

        public void PlayAll()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => ((IMenuItemViewModel)i).Item).ToList(), ClearCurrent = true });
            SelectedItems.Clear();
        }

        public void RemoveFromPlaylist()
        {
            _eventAggregator.Publish(new RemoveFromPlaylistMessage { Queue = SelectedItems.Select(x => (PlaylistItemViewModel) x).ToList() });
        }

        public void NavigateToPlaylist()
        {
            _navigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public void PlayPrevious()
        {
            _eventAggregator.Publish(new PlayPreviousMessage());
        }

        public void PlayNext()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void PlayPause()
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public void Stop()
        {
            _eventAggregator.Publish(new StopMessage());
        }

        private void ManageSelectedItemsHooks(INotifyCollectionChanged newCollection, INotifyCollectionChanged oldCollection)
        {
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= OnSelectedItemsChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += OnSelectedItemsChanged;
            }
        }

        private void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateIsOpened();
        }

        private void UpdateIsOpened()
        {
            NotifyOfPropertyChange(() => CanAddToPlaylist);
            NotifyOfPropertyChange(() => CanRemoveFromPlaylist);
            IsOpened = SelectedItems.Count != 0;
        }
    }
}
