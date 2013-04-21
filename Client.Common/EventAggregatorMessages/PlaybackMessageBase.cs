using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class PlaybackMessageBase
    {
        public PlaylistItem Item { get; set; }

        public PlaybackMessageBase(PlaylistItem item)
        {
            Item = item;
        }
    }
}