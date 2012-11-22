using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public static class SearchConverters
    {
        public static MenuItemViewModel AsMenuItemViewModel(this ExpandedArtist artist)
        {
            return new MenuItemViewModel
                       {
                           Type = "Artists",
                           Title = artist.Name,
                           Subtitle = string.Format("{0} albums", artist.AlbumCount),
                           Item = new MusicDirectoryChild
                                      {
                                          Id = artist.Id,
                                          IsDirectory = true
                                      }
                       };
        }

        public static MenuItemViewModel AsMenuItemViewModel(this Album album)
        {
            return new MenuItemViewModel
                       {
                           Type = "Albums",
                           Title = album.Name,
                           Subtitle = string.Format("{0} tracks", album.SongCount),
                           Item = new MusicDirectoryChild
                                      {
                                          Id = album.Id,
                                          IsDirectory = true
                                      }
                       };
        }

        public static MenuItemViewModel AsMenuItemViewModel(this Song song)
        {
            return new MenuItemViewModel
                       {
                           Type = "Songs",
                           Title = song.Title,
                           Subtitle = string.Format("Artist: {0}, Album: {1}", song.Artist, song.Album),
                           Item = new MusicDirectoryChild
                                      {
                                          Id = song.Id,
                                          IsDirectory = false
                                      }
                       };
        }
    }
}