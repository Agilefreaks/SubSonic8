namespace Subsonic8.BottomBar
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Services;
    using global::Common.ExtensionsMethods;
    using MugenInjection.Attributes;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Main;

    public abstract class BottomBarViewModelBase : Screen, IBottomBarViewModel
    {
        #region Fields

        private bool _isOpened;

        private ObservableCollection<object> _selectedItems;

        private bool _canDismiss;

        private IEventAggregator _eventAggregator;

        private IPlaylistManagementService _playlistManagementService;

        private IErrorDialogViewModel _errorDialogViewModel;

        #endregion

        #region Constructors and Destructors

        protected BottomBarViewModelBase()
        {
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
                if (value == _isOpened) return;
                _isOpened = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanDismiss
        {
            get
            {
                return _canDismiss;
            }

            set
            {
                if (value.Equals(_canDismiss)) return;
                _canDismiss = value;
                NotifyOfPropertyChange(() => CanDismiss);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return PlaylistManagementService.IsPlaying;
            }
        }

        public bool SelectionExists
        {
            get
            {
                return SelectedItems.Count > 0;
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
                ApplySelectionChanges();
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public ICustomFrameAdapter NavigationService { get; set; }

        [Inject]
        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }

            set
            {
                _eventAggregator = value;
                EventAggregator.Subscribe(this);
            }
        }

        [Inject]
        public IPlaylistManagementService PlaylistManagementService
        {
            get
            {
                return _playlistManagementService;
            }

            set
            {
                _playlistManagementService = value;
                HookPlaylistManagementService();
            }
        }

        [Inject]
        public IErrorDialogViewModel ErrorDialogViewModel
        {
            get
            {
                return _errorDialogViewModel;
            }

            set
            {
                if (Equals(value, _errorDialogViewModel)) return;
                _errorDialogViewModel = value;
                NotifyOfPropertyChange(() => ErrorDialogViewModel);
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
            CanDismiss = true;
            IsOpened = false;
            NavigationService.NavigateToViewModel<MainViewModel>();
        }

        public virtual void ClearSelection()
        {
            CanDismiss = true;
            SelectedItems.Clear();
            IsOpened = false;
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

        #endregion

        #region Methods

        protected virtual void OnSelectedItemsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ApplySelectionChanges();
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
                == PlaylistManagementService.GetPropertyName(() => PlaylistManagementService.IsPlaying))
            {
                NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        private void ApplySelectionChanges()
        {
            CanDismiss = SelectedItems.Count != 0;
            IsOpened = SelectedItems.Count != 0;
            NotifyOfPropertyChange(() => SelectionExists);
        }

        #endregion
    }
}