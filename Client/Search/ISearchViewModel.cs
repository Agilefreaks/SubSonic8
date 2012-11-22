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

        void PopulateMenuItems();
    }
}