namespace Client.Common.EventAggregatorMessages
{
    using Client.Common.Models;

    public class PausePlaybackMessage : PlaybackMessageBase
    {
        #region Constructors and Destructors

        public PausePlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }

        #endregion
    }
}