using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class ResumePlaybackMessage : PlaybackMessageBase
    {
        public ResumePlaybackMessage(PlaylistItem item)
            : base(item)
        {
        }
    }
}