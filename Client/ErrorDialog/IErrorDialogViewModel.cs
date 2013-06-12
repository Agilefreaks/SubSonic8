namespace Subsonic8.ErrorDialog
{
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

        void HandleError(string errorMessage);

        void ShareErrorDetails();

        void GoBack();

        void ShowSettings();

        #endregion
    }
}