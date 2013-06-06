/*
 * Implementation taken from code project article:
 * title:  A WinRT CollectionView class with Filtering and Sorting
 * author: Bernardo Castilho
 * date:   17 Jan 2013 
 * url:    http://www.codeproject.com/Articles/527686/A-WinRT-CollectionView-class-with-Filtering-and-So 
 */
namespace Common.ListCollectionView
{
    /// <summary>
    /// Implements a WinRT version of the IEditableCollectionView interface.
    /// </summary>
    public interface IEditableCollectionView
    {
        #region Public Properties

        bool CanAddNew { get; }

        bool CanCancelEdit { get; }

        bool CanRemove { get; }

        object CurrentAddItem { get; }

        object CurrentEditItem { get; }

        bool IsAddingNew { get; }

        bool IsEditingItem { get; }

        #endregion

        #region Public Methods and Operators

        object AddNew();

        void CancelEdit();

        void CancelNew();

        void CommitEdit();

        void CommitNew();

        void EditItem(object item);

        #endregion
    }
}