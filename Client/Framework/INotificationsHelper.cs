using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Subsonic8.Framework.Interfaces;

namespace Subsonic8.Framework
{
    public interface INotificationsHelper : IToastNotificationCapable, ITileNotificationCapable, IHandle<StartPlaybackMessage>
    {
    }
}