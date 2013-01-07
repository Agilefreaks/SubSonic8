using Caliburn.Micro;
using Subsonic8.Messages;

namespace Subsonic8.Framework.Services
{
    public interface IDialogNotificationService : INotificationService<DialogNotificationOptions>, IHandle<NotificationMessage>
    {
    }
}
