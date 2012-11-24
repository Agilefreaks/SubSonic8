using System.Collections.Generic;
using System.Linq;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public interface ISearchViewModel
    {
        SearchResultCollection Parameter { get; set; }

        List<IGrouping<string, MenuItemViewModel>> MenuItems { get; }

        List<MenuItemViewModel> MenuItemViewModels { get; }

        void PopulateMenuItems();

        void PopulateArtists(List<ExpandedArtist> artists);

        void PopulateAlbums(List<Client.Common.Models.Subsonic.Album> albums);
        
        void PopulateSongs(List<Song> songs);
    }
}