using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Playlists
{
    public interface IPlaylistViewModel : ICollectionViewModel<object>
    {
        void DeletePlaylist(int id);
    }
}