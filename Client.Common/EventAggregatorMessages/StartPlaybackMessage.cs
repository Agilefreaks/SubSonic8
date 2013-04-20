using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class StartPlaybackMessage : PlaybackMessageBase
    {
        public StartPlaybackMessage(PlaylistItem item) 
            : base(item)
        {
        }
    }
}