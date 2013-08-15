namespace Common.Mocks
{
    using System;
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public class MockVideoPlayer : MockPlayer, IVideoPlayer
    {
        public Func<PlaybackStateEventArgs> OnGetPlaybackTimeInfo { get; set; }

        public PlaybackStateEventArgs GetPlaybackTimeInfo()
        {
            return OnGetPlaybackTimeInfo != null ? OnGetPlaybackTimeInfo() : new PlaybackStateEventArgs();
        }
    }
}