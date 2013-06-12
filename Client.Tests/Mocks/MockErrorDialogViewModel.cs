namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Services;
    using Subsonic8.ErrorDialog;
    using Windows.ApplicationModel.DataTransfer;

    public class MockErrorDialogViewModel : IErrorDialogViewModel
    {
        private readonly IList<string> _handledErrors;

        #region Constructors and Destructors

        public MockErrorDialogViewModel(string message = "test")
        {
            Message = message;
            _handledErrors = new List<string>();
        }

        #endregion

        #region Public Properties

        public bool IsOpen { get; set; }

        public string Message { get; private set; }

        public int HandleErrorCallCount { get; set; }

        public IEnumerable<string> HandledErrors
        {
            get
            {
                return _handledErrors;
            }
        }

        #endregion

        #region Public Methods and Operators

        public string ExceptionString { get; private set; }

        public IWinRTWrappersService WinRTWrapperService { get; private set; }

        public void CloseDialog()
        {
        }

        public void HandleError(string errorMessage)
        {
            HandleErrorCallCount++;
            _handledErrors.Add(errorMessage);
        }

        public void HandleError(Exception error)
        {
            _handledErrors.Add(error.ToString());
        }

        public void ShareErrorDetails()
        {
        }

        public void GoBack()
        {
        }

        public void ShowSettings()
        {
        }

        public void OnShareRequested(DataRequest dataRequest)
        {
        }

        #endregion
    }
}