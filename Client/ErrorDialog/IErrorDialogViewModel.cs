namespace Subsonic8.ErrorDialog
{
    using System;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Results;
    using Client.Common.Services;

    public interface IErrorDialogViewModel : IErrorHandler, ISupportSharing
    {
        #region Public Properties

        string ExceptionString { get; }

        IWinRTWrappersService WinRTWrapperService { get; }

        #endregion

        #region Public Methods and Operators

        void ShareErrorDetails();

        void GoBack();

        void Show();

        void ShowSettings();

        #endregion

        void Hide();

        Task HandleCriticalError(Exception exception);
    }
}