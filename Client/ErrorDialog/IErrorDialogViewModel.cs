namespace Subsonic8.ErrorDialog
{
    using Caliburn.Micro;
    using Client.Common.Results;
    using Client.Common.Services;

    public interface IErrorDialogViewModel : IErrorHandler, ISupportSharing
    {
        #region Public Properties

        bool IsOpen { get; set; }

        string ErrorMessage { get; }

        IWinRTWrappersService WinRTWrapperService { get; }

        #endregion

        #region Public Methods and Operators

        void CloseDialog();

        void HandleError(string errorMessage);

        void ShareErrorDetails();

        #endregion
    }
}