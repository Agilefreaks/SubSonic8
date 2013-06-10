namespace Subsonic8.ErrorDialog
{
    using Caliburn.Micro;
    using Client.Common.Results;

    public interface IErrorDialogViewModel : IErrorHandler, ISupportSharing
    {
        #region Public Properties

        bool IsOpen { get; set; }

        string ErrorMessage { get; }

        ISharingService SharingService { get; set; }

        #endregion

        #region Public Methods and Operators

        void CloseDialog();

        void HandleError(string errorMessage);

        void ShareErrorDetails();

        #endregion
    }
}