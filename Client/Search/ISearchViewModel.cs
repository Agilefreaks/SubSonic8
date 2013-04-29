using System.Collections.Generic;
using System.Linq;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public interface ISearchViewModel : ICollectionViewModel<string>
    {
        SearchResultState State { get; }

        List<IGrouping<string, MenuItemViewModel>> GroupedMenuItems { get; }
    }
}