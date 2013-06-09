namespace Subsonic8.ErrorDialog
{
    using System;
    using System.ServiceModel;
    using System.Text;
    using Caliburn.Micro;
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

        private readonly DataTransferManager _dataTransferManager;

        private string _errorMessage;

        private bool _isOpen;

        #endregion

        #region Constructors and Destructors

        public ErrorDialogViewModel()
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += DataTransferManagerOnDataRequested;
        }

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

        #endregion

        #region Public Methods and Operators

        public void CloseDialog()
        {
            ClearErrorMessage();
            IsOpen = false;
        }

        public void HandleError(Exception error)
        {
            var message = error is CommunicationException ? error.Message : error.ToString();
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
            DataTransferManager.ShowShareUI();
        }

        #endregion

        #region Methods

        private void DataTransferManagerOnDataRequested(DataTransferManager sender, DataRequestedEventArgs eventArgs)
        {
            var dataRequest = eventArgs.Request;
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

        private void ClearErrorMessage()
        {
            ErrorMessage = null;
        }

        #endregion
    }
}