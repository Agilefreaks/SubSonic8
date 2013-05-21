namespace Client.Common.EventAggregatorMessages
{
    using Client.Common.Models;

    public class PlaybackMessageBase
    {
        #region Constructors and Destructors

        public PlaybackMessageBase(PlaylistItem item)
        {
            Item = item;
        }

        #endregion

        #region Public Properties

        public PlaylistItem Item { get; set; }

        #endregion
    }
}