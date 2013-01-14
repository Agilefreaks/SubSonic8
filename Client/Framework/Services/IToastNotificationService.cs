namespace Subsonic8.Framework.Services
{
    public interface IToastNotificationService : INotificationService<ToastNotificationOptions>
    {
        bool UseSound { get; set; }
    }
}