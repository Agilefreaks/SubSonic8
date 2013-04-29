using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using MugenInjection.Attributes;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;

namespace Subsonic8.Framework
{
    public class NotificationsHelper : INotificationsHelper
    {
        private IEventAggregator _eventAggregator;

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        [Inject]
        public ITileNotificationService TileNotificationService { get; set; }

        [Inject]
        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }

            set
            {
                _eventAggregator = value;
                _eventAggregator.Subscribe(this);
            }
        }

        public void Handle(StartPlaybackMessage message)
        {
            this.ShowToast(message.Item);
            this.UpdateTile(message.Item);
        }
    }
}