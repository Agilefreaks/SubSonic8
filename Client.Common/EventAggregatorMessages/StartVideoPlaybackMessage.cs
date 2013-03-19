using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class StartVideoPlaybackMessage
    {
        public PlaylistItem Item { get; set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public bool FullScreen { get; set; }

        public StartVideoPlaybackMessage(PlaylistItem item)
        {
            Item = item;
            EndTime = item.Duration;
            StartTime = 0;
        }
    }
}