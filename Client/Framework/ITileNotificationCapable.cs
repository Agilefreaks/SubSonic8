using Subsonic8.Framework.Services;

namespace Subsonic8.Framework
{
    public interface ITileNotificationCapable
    {
        ITileNotificationService TileNotificationService { get; }
    }
}