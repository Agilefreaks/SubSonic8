using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;

namespace Client.Common.Helpers
{
    public class MediaControlHandler : IMediaControlHandler
    {
        private readonly IEventAggregator _eventAggregator;

        public MediaControlHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void PlayPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public void PlayPausePressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public void PausePressed(object sender, object e)
        {
            _eventAggregator.Publish(new PausePlaybackMessage());
        }

        public void StopPressed(object sender, object e)
        {
            _eventAggregator.Publish(new StopPlaybackMessage());
        }

        public void PlayNextTrackPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void PlayPreviousTrackPressed(object sender, object e)
        {
            _eventAggregator.Publish(new PlayPreviousMessage());
        }
    }
}