namespace Subsonic8.Framework.Services
{
    public interface INotificationService<in TOptions>
    {
        void Show(TOptions options);
    }
}
