using Client.Common.Models;

namespace Client.Common.Services.DataStructures.PlayerManagementService
{
    public interface IPlayer
    {
        void Play(PlaylistItem item, object options = null);
        void Pause();
        void Resume();
        void Stop();
    }
}