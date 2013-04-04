using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Subsonic8.Messages;

namespace Subsonic8.Search
{
    public interface ISearchViewModel : IViewModel, IHandle<PerformSearch>
    {
        SearchResultCollection Parameter { get; set; }

        List<IGrouping<string, MenuItemViewModel>> MenuItems { get; }

        List<MenuItemViewModel> MenuItemViewModels { get; }

        SearchResultState State { get; set; }

        void PopulateMenuItems();

        void PopulateArtists(List<ExpandedArtist> artists);

        void PopulateAlbums(List<Client.Common.Models.Subsonic.Album> albums);
        
        void PopulateSongs(List<Song> songs);
    }
}