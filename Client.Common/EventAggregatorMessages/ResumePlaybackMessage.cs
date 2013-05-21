namespace Client.Common.EventAggregatorMessages
{
    using Client.Common.Models;

    public class ResumePlaybackMessage : PlaybackMessageBase
    {
        #region Constructors and Destructors

        public ResumePlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }

        #endregion
    }
}