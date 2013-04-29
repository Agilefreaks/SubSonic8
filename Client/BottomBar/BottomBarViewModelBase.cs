using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Caliburn.Micro;
using Client.Common;
using Client.Common.EventAggregatorMessages;
using Client.Common.Services;
using Subsonic8.Main;

namespace Subsonic8.BottomBar
{
    public abstract class BottomBarViewModelBase : Screen, IBottomBarViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IPlaylistManagementService PlaylistManagementService;
        protected readonly INavigationService NavigationService;

        private ObservableCollection<object> _selectedItems;
        private bool _isOpened;

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

        public bool IsPlaying
        {
            get
            {
                return PlaylistManagementService.IsPlaying;
            }
        }

        public bool ShuffleOn
        {
            get { return PlaylistManagementService.ShuffleOn; }
        }

        public bool DisplayPlayControls
        {
            get
            {
                return PlaylistManagementService.HasElements;
            }
        }

        protected BottomBarViewModelBase(INavigationService navigationService, IEventAggregator eventAggregator,
            IPlaylistManagementService playlistManagementService)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            EventAggregator.Subscribe(this);
            PlaylistManagementService = playlistManagementService;
            HookPlaylistManagementService();


            SelectedItems = new ObservableCollection<object>();
            SelectedItems.CollectionChanged += OnSelectedItemsChanged;
        }

        public virtual void Handle(PlaylistStateChangedMessage message)
        {
            NotifyOfPropertyChange(() => DisplayPlayControls);
        }

        public virtual void PlayPrevious()
        {
            EventAggregator.Publish(new PlayPreviousMessage());
        }

        public virtual void PlayNext()
        {
            EventAggregator.Publish(new PlayNextMessage());
        }

        public virtual void PlayPause()
        {
            EventAggregator.Publish(new PlayPauseMessage());
        }

        public virtual void Stop()
        {
            EventAggregator.Publish(new StopMessage());
        }

        public virtual void ToggleShuffle()
        {
            EventAggregator.Publish(new ToggleShuffleMessage());
        }

        public void NavigateToRoot()
        {
            NavigationService.NavigateToViewModel<MainViewModel>();
        }

        protected virtual void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateIsOpened();
        }

        private void HookPlaylistManagementService()
        {
            PlaylistManagementService.PropertyChanged += PlaylistManagementServiceOnPropertyChanged;
        }

        private void UpdateIsOpened()
        {
            IsOpened = SelectedItems.Count != 0;
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

        private void PlaylistManagementServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == PlaylistManagementService.GetPropertyName(() => PlaylistManagementService.ShuffleOn))
            {
                NotifyOfPropertyChange(() => ShuffleOn);
            }

            if (propertyChangedEventArgs.PropertyName == PlaylistManagementService.GetPropertyName(() => PlaylistManagementService.IsPlaying))
            {
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }
    }
}