namespace Client.Common.Helpers
{
    using Windows.Media;

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
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Public Methods and Operators

        public void Handle(StartPlaybackMessage message)
        {
            var artist = message.Item.Artist;
            var title = message.Item.Title;
            MediaControl.ArtistName = GetValueOrPlaceOrder(artist);
            MediaControl.TrackName = GetValueOrPlaceOrder(title);
        }

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

        #region Methods

        private static string GetValueOrPlaceOrder(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        #endregion
    }
}