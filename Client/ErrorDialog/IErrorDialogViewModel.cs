namespace Subsonic8.ErrorDialog
{
    using Client.Common.Results;

    public interface IErrorDialogViewModel : IErrorHandler
    {
        #region Public Properties

        bool IsOpen { get; set; }

        string ErrorMessage { get; }

        #endregion

        #region Public Methods and Operators

        void CloseDialog();

        void HandleError(string errorMessage);

        void ShareErrorDetails();

        #endregion
    }
}