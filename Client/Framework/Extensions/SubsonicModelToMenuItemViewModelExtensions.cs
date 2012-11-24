using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;

namespace Subsonic8.Framework.Extensions
{
    public static class SubsonicModelToMenuItemViewModelExtensions
    {
        public static MenuItemViewModel AsMenuItemViewModel(this ExpandedArtist artist)
        {
            return new MenuItemViewModel
                       {
                           Type = "Artists",
                           Title = artist.Name,
                           Subtitle = string.Format("{0} albums", artist.AlbumCount),
                           Item = artist
                       };
        }

        public static MenuItemViewModel AsMenuItemViewModel(this Client.Common.Models.Subsonic.Album album)
        {
            return new MenuItemViewModel
                       {
                           Type = "Albums",
                           Title = album.Name,
                           Subtitle = string.Format("{0} tracks", album.SongCount),
                           Item = album
                       };
        }

        public static MenuItemViewModel AsMenuItemViewModel(this Song song)
        {
            return new MenuItemViewModel
                       {
                           Type = "Songs",
                           Title = song.Title,
                           Subtitle = string.Format("Artist: {0}, Album: {1}", song.Artist, song.Album),
                           Item = song
                       };
        }
    }
}