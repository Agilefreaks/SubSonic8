namespace Client.Common.Services.DataStructures.PlayerManagementService
{
    using Client.Common.Models;

    public interface IPlayer
    {
        #region Public Methods and Operators

        void Pause();

        void Play(PlaylistItem item, object options = null);

        void Resume();

        void Stop();

        #endregion
    }
}