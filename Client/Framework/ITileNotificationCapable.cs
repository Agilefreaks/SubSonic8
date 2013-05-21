namespace Subsonic8.Framework
{
    using Subsonic8.Framework.Services;

    public interface ITileNotificationCapable
    {
        #region Public Properties

        ITileNotificationService TileNotificationService { get; }

        #endregion
    }
}