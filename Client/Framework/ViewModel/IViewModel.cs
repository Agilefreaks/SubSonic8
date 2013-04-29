using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.Framework.Services;
using Action = System.Action;

namespace Subsonic8.Framework.ViewModel
{
    public interface IViewModel : IScreen, ISongLoader
    {
        IEventAggregator EventAggregator { get; set; }

        ICustomFrameAdapter NavigationService { get; set; }

        IDialogNotificationService NotificationService { get; set; }

        bool CanGoBack { get; }

        void GoBack();

        Action UpdateDisplayName { get; set; }
    }
}