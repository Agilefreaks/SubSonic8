namespace Subsonic8.Framework.Services
{
    public interface INotificationService
    {
        void Show(NotificationOptions options);

        bool UseSound { get; set; }
    }
}
