namespace Subsonic8.Framework
{
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Subsonic8.Framework.Interfaces;

    public interface INotificationsHelper : IToastNotificationCapable, 
                                            ITileNotificationCapable, 
                                            IHandle<StartPlaybackMessage>
    {
    }
}