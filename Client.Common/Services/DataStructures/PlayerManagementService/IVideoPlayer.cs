namespace Client.Common.Services.DataStructures.PlayerManagementService
{
    public interface IVideoPlayer : IPlayer
    {
        PlaybackStateEventArgs GetPlaybackTimeInfo();
    }
}