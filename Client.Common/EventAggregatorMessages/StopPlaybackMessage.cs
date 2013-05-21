namespace Client.Common.EventAggregatorMessages
{
    using Client.Common.Models;

    public class StopPlaybackMessage : PlaybackMessageBase
    {
        #region Constructors and Destructors

        public StopPlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }

        #endregion
    }
}