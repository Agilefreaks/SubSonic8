namespace Subsonic8.Framework.Services
{
    public interface IToastNotificationService : INotificationService<PlaybackNotificationOptions>
    {
        bool UseSound { get; set; }
    }
}