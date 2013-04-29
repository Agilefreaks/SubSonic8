using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.Framework.Services;
using Action = System.Action;

namespace Subsonic8.Framework.ViewModel
{
    public interface IViewModel : IScreen, IErrorHandler
    {
        IEventAggregator EventAggregator { get; set; }

        ICustomFrameAdapter NavigationService { get; set; }

        ISubsonicService SubsonicService { get; set; }

        IDialogNotificationService NotificationService { get; set; }

        bool CanGoBack { get; }

        void GoBack();

        Action UpdateDisplayName { get; set; }
    }
}