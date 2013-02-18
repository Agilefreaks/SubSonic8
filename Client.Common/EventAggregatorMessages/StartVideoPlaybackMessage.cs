using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class StartVideoPlaybackMessage
    {
        public PlaylistItem Item { get; set; }

        public StartVideoPlaybackMessage(PlaylistItem item)
        {
            Item = item;
        }
    }
}