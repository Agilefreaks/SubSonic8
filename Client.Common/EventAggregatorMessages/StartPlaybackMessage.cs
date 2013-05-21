namespace Client.Common.EventAggregatorMessages
{
    using Client.Common.Models;

    public class StartPlaybackMessage : PlaybackMessageBase
    {
        #region Constructors and Destructors

        public StartPlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }

        #endregion

        #region Public Properties

        public object Options { get; set; }

        #endregion
    }
}