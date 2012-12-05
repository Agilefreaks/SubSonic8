using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.Playback;

namespace Subsonic8.BottomBar
{
    public class MediaSelectionBottomBarViewModel : Screen, IMediaSelectionBottomBarViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private bool _isOpened;
        private ObservableCollection<MenuItemViewModel> _selectedItems;

        public ObservableCollection<MenuItemViewModel> SelectedItems
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

        public MediaSelectionBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            SelectedItems = new ObservableCollection<MenuItemViewModel>();
        }

        public void AddToPlaylist()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => i.Item).ToList() });
            SelectedItems.Clear();
        }

        public void NavigateToPlaylist()
        {
            _navigationService.NavigateToViewModel<PlaybackViewModel>();
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
            IsOpened = SelectedItems.Count != 0;
        }
    }
}
