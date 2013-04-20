using Client.Common.Models;

namespace Client.Common.Services.DataStructures.PlayerManagementService
{
    public interface IPlayer
    {
        void Play(PlaylistItem item);
        void Pause();
        void Resume();
        void Stop();
    }
}