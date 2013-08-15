namespace Client.Common.Tests.Mocks
{
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public class MockVideoPlayer : MockPlayer, IVideoPlayer
    {
        public PlaybackStateEventArgs GetPlaybackTimeInfo()
        {
            return new PlaybackStateEventArgs();
        }
    }
}