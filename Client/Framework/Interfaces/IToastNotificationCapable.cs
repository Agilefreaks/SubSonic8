namespace Subsonic8.Framework.Interfaces
{
    using Subsonic8.Framework.Services;

    public interface IToastNotificationCapable
    {
        #region Public Properties

        IToastNotificationService ToastNotificationService { get; }

        #endregion
    }
}