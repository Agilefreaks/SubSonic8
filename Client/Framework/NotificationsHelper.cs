namespace Subsonic8.Framework
{
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Services;

    public class NotificationsHelper : INotificationsHelper
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        #endregion

        #region Public Properties

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

        [Inject]
        public ITileNotificationService TileNotificationService { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Handle(StartPlaybackMessage message)
        {
            this.ShowToast(message.Item);
            this.UpdateTile(message.Item);
        }

        #endregion
    }
}