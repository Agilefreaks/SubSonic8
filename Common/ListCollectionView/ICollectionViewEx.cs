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
    using System.Collections;
    using System.Collections.Generic;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// Extends the WinRT ICollectionView to provide sorting and filtering.
    /// </summary>
    public interface ICollectionViewEx : ICollectionView
    {
        #region Public Properties

        /// <summary>
        /// Gets a value that indicates whether this view supports filtering via the
        /// <see cref="Filter"/> property.
        /// </summary>
        bool CanFilter { get; }

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping via the 
        /// <see cref="GroupDescriptions"/> property.
        /// </summary>
        bool CanGroup { get; }

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting via the 
        /// <see cref="SortDescriptions"/> property.
        /// </summary>
        bool CanSort { get; }

        /// <summary>
        /// Gets or sets a callback used to determine if an item is suitable for inclusion
        /// in the view.
        /// </summary>
        Predicate<object> Filter { get; set; }

        /// <summary>
        /// Gets a collection of System.ComponentModel.GroupDescription objects that
        /// describe how the items in the collection are grouped in the view.
        /// </summary>
        IList<object> GroupDescriptions { get; }

        /// <summary>
        /// Gets a collection of System.ComponentModel.SortDescription objects that describe
        /// how the items in the collection are sorted in the view.
        /// </summary>
        IList<SortDescription> SortDescriptions { get; }

        /// <summary>
        /// Get the underlying collection.
        /// </summary>        
        IEnumerable SourceCollection { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay
        /// automatic refresh.
        /// </summary>
        IDisposable DeferRefresh();

        /// <summary>
        /// Refreshes the view applying the current sort and filter conditions.
        /// </summary>
        void Refresh();

        #endregion
    }
}