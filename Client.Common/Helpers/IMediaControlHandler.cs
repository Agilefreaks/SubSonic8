namespace Client.Common.Helpers
{
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;

    public interface IMediaControlHandler : IHandle<StartPlaybackMessage>
    {
        #region Public Methods and Operators

        void PausePressed(object sender, object e);

        void PlayNextTrackPressed(object sender, object e);

        void PlayPausePressed(object sender, object e);

        void PlayPressed(object sender, object e);

        void PlayPreviousTrackPressed(object sender, object e);

        void StopPressed(object sender, object e);

        #endregion
    }
}