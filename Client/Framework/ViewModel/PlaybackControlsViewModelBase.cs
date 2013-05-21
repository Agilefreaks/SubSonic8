namespace Subsonic8.Framework.ViewModel
{
    using Client.Common.EventAggregatorMessages;

    public abstract class PlaybackControlsViewModelBase : ViewModelBase, IPlaybackControlsViewModel
    {
        #region Public Methods and Operators

        public virtual void Next()
        {
            EventAggregator.Publish(new PlayNextMessage());
        }

        public virtual void PlayPause()
        {
            EventAggregator.Publish(new PlayPauseMessage());
        }

        public virtual void Previous()
        {
            EventAggregator.Publish(new PlayPreviousMessage());
        }

        public virtual void Stop()
        {
            EventAggregator.Publish(new StopMessage());
        }

        #endregion

        #region Methods

        protected override void OnEventAggregatorSet()
        {
            base.OnEventAggregatorSet();
            EventAggregator.Subscribe(this);
        }

        #endregion
    }
}