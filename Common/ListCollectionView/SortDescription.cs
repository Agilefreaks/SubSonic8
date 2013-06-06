/*
 * Implementation taken from code project article:
 * title:  A WinRT CollectionView class with Filtering and Sorting
 * author: Bernardo Castilho
 * date:   17 Jan 2013 
 * url:    http://www.codeproject.com/Articles/527686/A-WinRT-CollectionView-class-with-Filtering-and-So 
 */
namespace Common.ListCollectionView
{
    public class SortDescription
    {
        #region Constructors and Destructors

        public SortDescription(string propertyName, ListSortDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        #endregion

        #region Public Properties

        public ListSortDirection Direction { get; set; }

        public string PropertyName { get; set; }

        #endregion
    }
}