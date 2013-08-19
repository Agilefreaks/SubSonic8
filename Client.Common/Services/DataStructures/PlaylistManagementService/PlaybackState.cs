namespace Client.Common.Services.DataStructures.PlaylistManagementService
{
    using Client.Common.Models;

    public class PlaylistServiceState
    {
        public PlaylistItemCollection Items { get; set; }

        public bool IsShuffleOn { get; set; }

        public int CurrentTrackNumber { get; set; }

        public bool IsPlaying { get; set; }
    }
}