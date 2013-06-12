namespace Subsonic8.ErrorDialog
{
    using System;
    using System.Text;
    using Caliburn.Micro;
    using Client.Common.Services;
    using Subsonic8.Framework.Services;
    using Subsonic8.Settings;
    using Windows.ApplicationModel.DataTransfer;

    public class ErrorDialogViewModel : Screen, IErrorDialogViewModel
    {
        #region Constants

        private const string ErrorReportDeliveryEmail = "subsonic8@agilefreaks.com";

        private const string ActionInormation = "Please send this email to: " + ErrorReportDeliveryEmail;

        private const string TitleMessage = "Could not complete request.";

        private const string DefaultMessage = "You can choose to send us a report with the error details or ignore this.";

        #endregion

        #region Fields

        private string _exceptionString;

        private string _errorDescription;

        #endregion

        #region Constructors and Destructors

        public ErrorDialogViewModel(IWinRTWrappersService winRTWrappersService, INavigationService navigationService)
        {
            WinRTWrapperService = winRTWrappersService;
            WinRTWrapperService.RegisterShareRequestHandler(OnShareRequested);
            NavigationService = navigationService;
            NavigateAction = Navigate;
        }

        #endregion

        #region Public Properties

        public string Message
        {
            get
            {
                return DefaultMessage;
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

        public Action<Type> NavigateAction { get; set; }

        #endregion

        #region Public Methods and Operators

        public void HandleError(Exception error)
        {
            ExceptionString = error.ToString();
            HandleError(error.Message);
        }

        public void HandleError(string errorMessage)
        {
            ErrorDescription = errorMessage;
            NavigateAction(typeof(ErrorDialogViewModel));
        }

        public void ShareErrorDetails()
        {
            WinRTWrapperService.ShowShareUI();
        }

        public void GoBack()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
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
                dataBuilder.Append(string.Format("<h2>{0}</h2><br/>", ActionInormation));
                dataBuilder.Append(string.Format("<div><span>{0}</span></div>", ExceptionString));
                dataRequest.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(dataBuilder.ToString()));
            }
        }

        public void ShowSettings()
        {
            DialogService.ShowSettings<SettingsViewModel>();
        }

        public void Navigate(Type type)
        {
            NavigationService.NavigateToViewModel(type);
        }

        #endregion

        #region Methods

        protected override void OnActivate()
        {
            base.OnActivate();
            DisplayName = TitleMessage;
        }

        #endregion
    }
}