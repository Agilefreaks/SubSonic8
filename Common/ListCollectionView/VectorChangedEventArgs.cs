/*
 * Implementation taken from code project article:
 * title:  A WinRT CollectionView class with Filtering and Sorting
 * author: Bernardo Castilho
 * date:   17 Jan 2013 
 * url:    http://www.codeproject.com/Articles/527686/A-WinRT-CollectionView-class-with-Filtering-and-So 
 */
namespace Common.ListCollectionView
{
    using Windows.Foundation.Collections;

    /// <summary>
    /// Class that implements IVectorChangedEventArgs so we can fire VectorChanged events.
    /// </summary>
    internal class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        #region Fields

        private readonly CollectionChange _collectionChange = CollectionChange.Reset;

        private readonly uint _index = 0xffff;

        #endregion

        #region Constructors and Destructors

        public VectorChangedEventArgs(CollectionChange collectionChange, int index = -1)
        {
            _collectionChange = collectionChange;
            _index = (uint)index;
        }

        #endregion

        #region Public Properties

        public CollectionChange CollectionChange
        {
            get
            {
                return _collectionChange;
            }
        }

        public uint Index
        {
            get
            {
                return _index;
            }
        }

        #endregion
    }
}