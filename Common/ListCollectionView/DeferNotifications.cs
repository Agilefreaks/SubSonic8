/*
 * Implementation taken from code project article:
 * title:  A WinRT CollectionView class with Filtering and Sorting
 * author: Bernardo Castilho
 * date:   17 Jan 2013 
 * url:    http://www.codeproject.com/Articles/527686/A-WinRT-CollectionView-class-with-Filtering-and-So 
 */
namespace Common.ListCollectionView
{
    using System;

    /// <summary>
    /// Class that handles deferring notifications while the view is modified.
    /// </summary>
    internal class DeferNotifications : IDisposable
    {
        #region Fields

        private readonly object _currentItem;

        private readonly IUpdateTracker _updateTracker;

        private readonly ListCollectionView _view;

        #endregion

        #region Constructors and Destructors

        internal DeferNotifications(ListCollectionView view, IUpdateTracker updateTracker)
        {
            _view = view;
            _updateTracker = updateTracker;
            _updateTracker = updateTracker;
            _currentItem = _view.CurrentItem;
            _updateTracker.RunningUpdateCount++;
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            _view.MoveCurrentTo(_currentItem);
            _updateTracker.RunningUpdateCount--;
            _view.Refresh();
        }

        #endregion
    }
}