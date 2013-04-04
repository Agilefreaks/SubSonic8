using Caliburn.Micro;

namespace Client.Common.Services
{
    public interface ICustomFrameAdapter : INavigationService
    {
        void NavigateToViewModel<T>(object parameter = null)
            where T : Screen;
    }
}