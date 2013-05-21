namespace Client.Common.Helpers
{
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;

    public class MediaControlHandler : IMediaControlHandler
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public MediaControlHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region Public Methods and Operators

        public void PausePressed(object sender, object e)
        {
            _eventAggregator.Publish(new PauseMessage());
        }

        public void PlayNextTrackPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void PlayPausePressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public void PlayPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayMessage());
        }

        public void PlayPreviousTrackPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayPreviousMessage());
        }

        public void StopPressed(object sender, object e)
        {
            _eventAggregator.Publish(new StopMessage());
        }

        #endregion
    }
}