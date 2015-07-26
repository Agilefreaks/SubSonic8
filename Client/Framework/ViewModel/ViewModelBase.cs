namespace Subsonic8.Framework.ViewModel
{
    using Caliburn.Micro;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework.Services;
    using global::Common.Interfaces;
    using Action = System.Action;

    public abstract class ViewModelBase : Screen, IViewModel
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        private ICustomFrameAdapter _navigationService;

        private ISubsonicService _subsonicService;

        #endregion

        #region Constructors and Destructors

        protected ViewModelBase()
        {
            UpdateDisplayName = () => DisplayName = "Subsonic8";
        }

        #endregion

        #region Public Properties

        public bool CanGoBack
        {
            get
            {
                return NavigationService != null && NavigationService.CanGoBack;
            }
        }

        public IErrorHandler ErrorHandler
        {
            get
            {
                return ErrorDialogViewModel;
            }
        }

        [Inject]
        public IErrorDialogViewModel ErrorDialogViewModel { get; set; }

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
                OnEventAggregatorSet();
            }
        }

        [Inject]
        public ICustomFrameAdapter NavigationService
        {
            get
            {
                return _navigationService;
            }

            set
            {
                _navigationService = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanGoBack);
            }
        }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        [Inject]
        public ISubsonicService SubsonicService
        {
            get
            {
                return _subsonicService;
            }

            set
            {
                _subsonicService = value;
                NotifyOfPropertyChange();
            }
        }

        public Action UpdateDisplayName { get; set; }

        #endregion

        #region Public Methods and Operators

        public void GoBack()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region Methods

        protected override void OnActivate()
        {
            base.OnActivate();

            UpdateDisplayName();
        }

        protected virtual void OnEventAggregatorSet()
        {
        }

        #endregion
    }
}