namespace Subsonic8.Framework.Services
{
    public interface IToastNotificationService : INotificationService<PlaybackNotificationOptions>
    {
        #region Public Properties

        bool UseSound { get; set; }

        #endregion
    }
}