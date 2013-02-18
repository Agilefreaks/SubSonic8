using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Action = System.Action;

namespace Subsonic8.BottomBar
{
    public class DefaultBottomBarViewModel : Screen, IDefaultBottomBarViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private bool _isOpened;
        private bool _isPlaying;
        private bool _isOnPlaylist;
        private ObservableCollection<object> _selectedItems;
        private bool _displayPlayControls;

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

        private IEnumerable<ISubsonicModel> SelectedSubsonicItems
        {
            get { return SelectedItems.Cast<IMenuItemViewModel>().Select(vm => vm.Item); }
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

        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }

            set
            {
                _isPlaying = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsOnPlaylist
        {
            get
            {
                return _isOnPlaylist;
            }

            set
            {
                _isOnPlaylist = value;
                NotifyOfPropertyChange();
            }
        }

        public bool DisplayPlayControls
        {
            get { return _displayPlayControls; }

            set
            {
                _displayPlayControls = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanAddToPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(MenuItemViewModel)); }
        }

        public bool CanRemoveFromPlaylist
        {
            get { return SelectedItems.Any() && SelectedItems.All(x => x.GetType() == typeof(Client.Common.Models.PlaylistItem)); }
        }

        public Action Navigate { get; set; }

        public DefaultBottomBarViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            SelectedItems = new ObservableCollection<object>();
            SelectedItems.CollectionChanged += OnSelectedItemsChanged;
            Navigate = () => _navigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public void AddToPlaylist()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedSubsonicItems.ToList() });
            SelectedItems.Clear();
        }

        public void PlayAll()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => ((IMenuItemViewModel)i).Item).ToList(), ClearCurrent = true });
            SelectedItems.Clear();
            Navigate();
        }

        public void RemoveFromPlaylist()
        {
            _eventAggregator.Publish(new RemoveItemsMessage { Queue = SelectedItems.Select(x => (Client.Common.Models.PlaylistItem)x).ToList() });
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

        public void ToggleShuffle()
        {
            _eventAggregator.Publish(new ToggleShuffleMessage());
        }

        public void Stop()
        {
            _eventAggregator.Publish(new StopMessage());
        }

        public void Handle(ShowControlsMessage message)
        {
            DisplayPlayControls = message.Show;
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
