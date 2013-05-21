namespace Subsonic8.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.MenuItem;

    public interface ISearchViewModel : ICollectionViewModel<string>
    {
        #region Public Properties

        List<IGrouping<string, MenuItemViewModel>> GroupedMenuItems { get; }

        SearchResultState State { get; }

        #endregion
    }
}