namespace Subsonic8.ErrorDialog
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Services;
    using Subsonic8.Framework.Services;
    using Subsonic8.Settings;
    using Windows.ApplicationModel.DataTransfer;

    public class ErrorDialogViewModel : Screen, IErrorDialogViewModel
    {
        private readonly IDialogNotificationService _dialogNotificationService;

        private readonly IResourceService _resourceService;

        private readonly IDialogService _dialogService;

        #region Fields

        private string _exceptionString;

        private string _errorDescription;

        private Exception _error;

        private bool _isHidden;

        private string _notice;

        #endregion

        #region Constructors and Destructors

        public ErrorDialogViewModel(
            IWinRTWrappersService winRTWrappersService,
            INavigationService navigationService,
            IDialogNotificationService dialogNotificationService,
            IResourceService resourceService,
            IDialogService dialogService)
        {
            _dialogNotificationService = dialogNotificationService;
            _resourceService = resourceService;
            _dialogService = dialogService;
            IsHidden = true;
            WinRTWrapperService = winRTWrappersService;
            WinRTWrapperService.RegisterShareRequestHandler(OnShareRequested);
            NavigationService = navigationService;
            DisplayName = "It seems that something went wrong ;(";
        }

        #endregion

        #region Public Properties

        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }

            set
            {
                if (value.Equals(_isHidden)) return;
                _isHidden = value;
                NotifyOfPropertyChange(() => IsHidden);
            }
        }

        public string Notice
        {
            get
            {
                return _resourceService.GetStringResource("ErrorDialogViewModelStrings/Notice");
            }
        }

        public string Message
        {
            get
            {
                return _resourceService.GetStringResource("ErrorDialogViewModelStrings/DefaultMessage");
            }
        }

        public string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }

            set
            {
                if (value == _errorDescription)
                {
                    return;
                }

                _errorDescription = value;
                NotifyOfPropertyChange();
            }
        }

        public string ExceptionString
        {
            get
            {
                return _exceptionString;
            }

            private set
            {
                if (value == _exceptionString)
                {
                    return;
                }

                _exceptionString = value;
                NotifyOfPropertyChange();
            }
        }

        public IWinRTWrappersService WinRTWrapperService { get; private set; }

        public INavigationService NavigationService { get; private set; }

        #endregion

        #region Public Methods and Operators

        public async Task HandleError(Exception error)
        {
            _error = error;
            ExceptionString = error.ToString();
            ErrorDescription = error.Message;
            Show();

            await Task.Run(() => { });
        }

        public void ShareErrorDetails()
        {
            WinRTWrapperService.ShowShareUI();
        }

        public void ReportErrorDetails()
        {
            string message;
            try
            {
                BugFreak.ReportingService.Instance.BeginReport(_error);
                message = _resourceService.GetStringResource("ErrorDialogViewModelStrings/ErrorDelivered");
            }
            catch (Exception)
            {
                message = _resourceService.GetStringResource("ErrorDialogViewModelStrings/ErrorWasNotDelivered");
            }

            _dialogNotificationService.Show(new DialogNotificationOptions { Message = message });
        }

        public void GoBack()
        {
            Hide();
        }

        public void OnShareRequested(DataRequest dataRequest)
        {
            if (string.IsNullOrWhiteSpace(_exceptionString))
            {
                dataRequest.FailWithDisplayText("There is nothing to share at this moment.");
            }
            else
            {
                dataRequest.Data.Properties.Title = "Error report";
                dataRequest.Data.Properties.Description = "Share this with us by email";
                dataRequest.Data.Properties.ApplicationName = "Subsonic8";
                var dataBuilder = new StringBuilder();
                var actionInformation =
                    string.Format(
                        _resourceService.GetStringResource("ErrorDialogViewModelStrings/ActionInormation"),
                        _resourceService.GetStringResource("ErrorDialogViewModelStrings/ErrorReportDeliveryEmail"));
                dataBuilder.Append(string.Format("<h2>{0}</h2><br/>", actionInformation));
                dataBuilder.Append(string.Format("<div><span>{0}</span></div>", ExceptionString));
                dataRequest.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(dataBuilder.ToString()));
            }
        }

        public void ShowSettings()
        {
            _dialogService.ShowSettings<SettingsViewModel>();
        }

        public void Hide()
        {
            IsHidden = true;
        }

        public void Show()
        {
            IsHidden = false;
        }

        #endregion

        #region Methods

        protected override void OnActivate()
        {
            base.OnActivate();
            DisplayName = _resourceService.GetStringResource("ErrorDialogViewModelStrings/TitleMessage");
        }

        #endregion
    }
}