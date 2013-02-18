using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class StartAudioPlaybackMessage
    {
        public PlaylistItem Item { get; set; }

        public StartAudioPlaybackMessage(PlaylistItem item)
        {
            Item = item;
        }
    }
}