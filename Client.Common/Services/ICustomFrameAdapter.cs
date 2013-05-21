namespace Client.Common.Services
{
    using Caliburn.Micro;

    public interface ICustomFrameAdapter : INavigationService
    {
        #region Public Methods and Operators

        void NavigateToViewModel<T>(object parameter = null) where T : Screen;

        #endregion
    }
}