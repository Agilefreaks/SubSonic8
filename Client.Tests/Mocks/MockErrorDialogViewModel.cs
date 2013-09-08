namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.Common.Services;
    using Subsonic8.ErrorDialog;
    using Windows.ApplicationModel.DataTransfer;

    public class MockErrorDialogViewModel : IErrorDialogViewModel
    {
        private readonly IList<Exception> _handledErrors;

        #region Constructors and Destructors

        public MockErrorDialogViewModel(string message = "test")
        {
            Message = message;
            _handledErrors = new List<Exception>();
        }

        #endregion

        #region Public Properties

        public bool IsOpen { get; set; }

        public string Message { get; private set; }

        public IEnumerable<Exception> HandledErrors
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

        public async Task HandleError(Exception error)
        {
            _handledErrors.Add(error);

            await Task.Run(() => { });
        }

        public void ShareErrorDetails()
        {
        }

        public void GoBack()
        {
        }

        public void Show()
        {
        }

        public void ShowSettings()
        {
        }

        public void Hide()
        {
        }

        public void OnShareRequested(DataRequest dataRequest)
        {
        }

        #endregion
    }
}