using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class PlaybackControlsViewModelBase : ViewModelBase, IPlaybackControlsViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        protected PlaybackControlsViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public virtual void Next()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public virtual void Previous()
        {
            _eventAggregator.Publish(new PlayPreviousMessage());
        }

        public virtual void PlayPause()
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public virtual void Stop()
        {
            _eventAggregator.Publish(new StopPlaybackMessage());
        }
    }
}