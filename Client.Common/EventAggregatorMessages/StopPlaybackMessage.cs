using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class StopPlaybackMessage : PlaybackMessageBase
    {
        public StopPlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }
    }
}