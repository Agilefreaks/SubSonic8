namespace Subsonic8.BottomBar
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Services;
    using global::Common.ExtensionsMethods;
    using Subsonic8.Main;

    public abstract class BottomBarViewModelBase : Screen, IBottomBarViewModel
    {
        #region Fields

        protected readonly IEventAggregator EventAggregator;

        protected readonly INavigationService NavigationService;

        protected readonly IPlaylistManagementService PlaylistManagementService;

        private bool _isOpened;

        private ObservableCollection<object> _selectedItems;

        #endregion

        #region Constructors and Destructors

        protected BottomBarViewModelBase(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IPlaylistManagementService playlistManagementService)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            EventAggregator.Subscribe(this);
            PlaylistManagementService = playlistManagementService;
            HookPlaylistManagementService();
            SelectedItems = new ObservableCollection<object>();
        }

        #endregion

        #region Public Properties

        public bool DisplayPlayControls
        {
            get
            {
                return PlaylistManagementService.HasElements;
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

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return _selectedItems;
            }

            set
            {
                if (_selectedItems == value)
                {
                    return;
                }

                ManageSelectedItemsHooks(value, _selectedItems);
                _selectedItems = value;
                UpdateIsOpened();
                NotifyOfPropertyChange();
            }
        }

        public bool ShuffleOn
        {
            get
            {
                return PlaylistManagementService.ShuffleOn;
            }
        }

        #endregion

        #region Public Methods and Operators

        public virtual void Handle(PlaylistStateChangedMessage message)
        {
            NotifyOfPropertyChange(() => DisplayPlayControls);
        }

        public void NavigateToRoot()
        {
            NavigationService.NavigateToViewModel<MainViewModel>();
        }

        public virtual void PlayNext()
        {
            EventAggregator.Publish(new PlayNextMessage());
        }

        public virtual void PlayPause()
        {
            EventAggregator.Publish(new PlayPauseMessage());
        }

        public virtual void PlayPrevious()
        {
            EventAggregator.Publish(new PlayPreviousMessage());
        }

        public virtual void Stop()
        {
            EventAggregator.Publish(new StopMessage());
        }

        public virtual void ToggleShuffle()
        {
            EventAggregator.Publish(new ToggleShuffleMessage());
        }

        #endregion

        #region Methods

        protected virtual void OnSelectedItemsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateIsOpened();
        }

        private void HookPlaylistManagementService()
        {
            PlaylistManagementService.PropertyChanged += PlaylistManagementServiceOnPropertyChanged;
        }

        private void ManageSelectedItemsHooks(
            INotifyCollectionChanged newCollection, INotifyCollectionChanged oldCollection)
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

        private void PlaylistManagementServiceOnPropertyChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName
                == PlaylistManagementService.GetPropertyName(() => PlaylistManagementService.ShuffleOn))
            {
                NotifyOfPropertyChange(() => ShuffleOn);
            }

            if (propertyChangedEventArgs.PropertyName
                == PlaylistManagementService.GetPropertyName(() => PlaylistManagementService.IsPlaying))
            {
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        private void UpdateIsOpened()
        {
            IsOpened = SelectedItems.Count != 0;
        }

        #endregion
    }
}