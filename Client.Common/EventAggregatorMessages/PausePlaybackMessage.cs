using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class PausePlaybackMessage : PlaybackMessageBase
    {
        public PausePlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }
    }
}