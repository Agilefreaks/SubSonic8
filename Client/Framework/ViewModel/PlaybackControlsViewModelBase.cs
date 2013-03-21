using Client.Common.EventAggregatorMessages;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class PlaybackControlsViewModelBase : ViewModelBase, IPlaybackControlsViewModel
    {
        public virtual void Next()
        {
            EventAggregator.Publish(new PlayNextMessage());
        }

        public virtual void Previous()
        {
            EventAggregator.Publish(new PlayPreviousMessage());
        }

        public virtual void PlayPause()
        {
            EventAggregator.Publish(new PlayPauseMessage());
        }

        public virtual void Stop()
        {
            EventAggregator.Publish(new StopPlaybackMessage());
        }

        protected override void OnEventAggregatorSet()
        {
            base.OnEventAggregatorSet();
            EventAggregator.Subscribe(this);
        }
    }
}