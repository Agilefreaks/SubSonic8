namespace Client.Common.Helpers
{
    public interface IMediaControlHandler
    {
        void PlayPressed(object sender, object e);

        void PlayPausePressed(object sender, object e);

        void PausePressed(object sender, object e);

        void StopPressed(object sender, object e);

        void PlayNextTrackPressed(object sender, object e);

        void PlayPreviousTrackPressed(object sender, object e);
    }
}