namespace Subsonic8.ErrorDialog
{
    using System;
    using System.Text;
    using Caliburn.Micro;
    using MugenInjection.Attributes;
    using Windows.ApplicationModel.DataTransfer;

    public class ErrorDialogViewModel : PropertyChangedBase, IErrorDialogViewModel
    {
        #region Constants

        private const string ErrorReportDeliveryEmail = "subsonic8@agilefreaks.com";

        private const string ActionInormation = "Please send this email to: " + ErrorReportDeliveryEmail;

        private const string TitleMessage = "An Error Was Encountered.";

        private const string DefaultMessage = "You can choose to send us a report with the error details or ignore this.";

        #endregion

        #region Fields

        private string _errorMessage;

        private bool _isOpen;

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        public string Title
        {
            get
            {
                return TitleMessage;
            }
        }

        public string Message
        {
            get
            {
                return DefaultMessage;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            private set
            {
                if (value == _errorMessage)
                {
                    return;
                }

                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            set
            {
                if (value.Equals(_isOpen))
                {
                    return;
                }

                _isOpen = value;
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public ISharingService SharingService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void HandleError(Exception error)
        {
            var message = error.ToString();
            HandleError(message);
        }

        public void HandleError(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsOpen = true;
        }

        public void ShareErrorDetails()
        {
            IsOpen = false;
            SharingService.ShowShareUI();
        }

        public void CloseDialog()
        {
            IsOpen = false;
            ClearErrorMessage();
        }

        public void OnShareRequested(DataRequest dataRequest)
        {
            if (string.IsNullOrWhiteSpace(_errorMessage))
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
                dataBuilder.Append(string.Format("<div><span>{0}</span></div>", ErrorMessage));
                dataRequest.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(dataBuilder.ToString()));
                ClearErrorMessage();
            }
        }

        #endregion

        #region Methods

        private void ClearErrorMessage()
        {
            ErrorMessage = null;
        }

        #endregion
    }
}