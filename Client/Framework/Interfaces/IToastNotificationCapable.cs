using Subsonic8.Framework.Services;

namespace Subsonic8.Framework.Interfaces
{
    public interface IToastNotificationCapable
    {
        IToastNotificationService ToastNotificationService { get; }
    }
}
