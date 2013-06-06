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
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// Simple implementation of the <see cref="ICollectionViewEx"/> interface, 
    /// which extends the standard WinRT definition of the <see cref="ICollectionView"/> 
    /// interface to add sorting and filtering.
    /// </summary>
    /// <remarks>
    /// Here's an example that shows how to use the <see cref="ListCollectionView"/> class:
    /// <code>
    /// // create a simple list
    /// var list = new List&lt;Rect&gt;();
    /// for (int i = 0; i &lt; 200; i++)
    ///   list.Add(new Rect(i, i, i, i));
    ///   
    /// // create collection view based on list
    /// var cv = new ListCollectionView();
    /// cv.Source = list;
    /// 
    /// // apply filter
    /// cv.Filter = (item) =&gt; { return ((Rect)item).X % 2 == 0; };
    /// 
    /// // apply sort
    /// cv.SortDescriptions.Add(new SortDescription("Width", ListSortDirection.Descending));
    /// 
    /// // show data on grid
    /// mygrid.ItemsSource = cv;
    /// </code>
    /// </remarks>
    public class ListCollectionView : ICollectionViewEx,
                                      IEditableCollectionView,
                                      INotifyPropertyChanged,
                                      IComparer<object>,
                                      IUpdateTracker
    {
        #region Fields

        private readonly ObservableCollection<SortDescription> _sort; // sorting parameters

        private readonly Dictionary<string, PropertyInfo> _sortProps; // PropertyInfo dictionary used while sorting

        private readonly List<object> _view; // filtered/sorted data source

        private Predicate<object> _filter; // filter

        private Type _itemType; // type of item in the source collection

        private int _runningUpdateCount;

        private object _source; // original data source

        private IList _sourceList; // original data source as list

        private INotifyCollectionChanged _sourceNcc; // listen to changes in the source

        #endregion

        #region Constructors and Destructors

        public ListCollectionView(object source)
        {
            // view exposed to consumers
            _view = new List<object>();

            // sort descriptor collection
            _sort = new ObservableCollection<SortDescription>();
            _sort.CollectionChanged += _sort_CollectionChanged;
            _sortProps = new Dictionary<string, PropertyInfo>();

            // hook up to data source
            Source = source;
        }

        public ListCollectionView()
            : this(null)
        {
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs after the current item has changed.
        /// </summary>
        public event EventHandler<object> CurrentChanged;

        /// <summary>
        /// Occurs before the current item changes.
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the view collection changes.
        /// </summary>
        public event VectorChangedEventHandler<object> VectorChanged;

        #endregion

        #region Public Properties

        public bool CanAddNew
        {
            get
            {
                return !IsReadOnly && _itemType != null;
            }
        }

        public bool CanCancelEdit
        {
            get
            {
                return true;
            }
        }

        public bool CanFilter
        {
            get
            {
                return true;
            }
        }

        public bool CanGroup
        {
            get
            {
                return false;
            }
        }

        public bool CanRemove
        {
            get
            {
                return !IsReadOnly;
            }
        }

        public bool CanSort
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a colletion of top level groups.
        /// </summary>
        public IObservableVector<object> CollectionGroups
        {
            get
            {
                return null;
            }
        }

        public int Count
        {
            get
            {
                return _view.Count;
            }
        }

        public object CurrentAddItem { get; private set; }

        public object CurrentEditItem { get; private set; }

        /// <summary>
        /// Gets the current item in the view.
        /// </summary>
        public object CurrentItem
        {
            get
            {
                return CurrentPosition > -1 && CurrentPosition < _view.Count ? _view[CurrentPosition] : null;
            }

            set
            {
                MoveCurrentTo(value);
            }
        }

        /// <summary>
        /// Gets the ordinal position of the current item in the view.
        /// </summary>
        public int CurrentPosition { get; private set; }

        public Predicate<object> Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    Refresh();
                }
            }
        }

        public IList<object> GroupDescriptions
        {
            get
            {
                return null;
            }
        }

        public bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        public bool IsAddingNew
        {
            get
            {
                return CurrentAddItem != null;
            }
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                return CurrentPosition >= _view.Count;
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                return CurrentPosition < 0;
            }
        }

        public bool IsEditingItem
        {
            get
            {
                return CurrentEditItem != null;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _sourceList == null || _sourceList.IsReadOnly;
            }
        }

        public IList<SortDescription> SortDescriptions
        {
            get
            {
                return _sort;
            }
        }

        /// <summary>
        /// Gets or sets the collection from which to create the view.
        /// </summary>
        public object Source
        {
            get
            {
                return _source;
            }

            set
            {
                if (_source != value)
                {
                    // save new source
                    _source = value;

                    // save new source as list (so we can add/remove etc)
                    _sourceList = value as IList;

                    // get the type of object in the collection
                    _itemType = GetItemType();

                    // listen to changes in the source
                    if (_sourceNcc != null)
                    {
                        _sourceNcc.CollectionChanged -= _sourceCollectionChanged;
                    }

                    _sourceNcc = _source as INotifyCollectionChanged;
                    if (_sourceNcc != null)
                    {
                        _sourceNcc.CollectionChanged += _sourceCollectionChanged;
                    }

                    // refresh our view
                    HandleSourceChanged();

                    // inform listeners
                    OnPropertyChanged("Source");
                }
            }
        }

        public IEnumerable SourceCollection
        {
            get
            {
                return _source as IEnumerable;
            }
        }

        #endregion

        #region Explicit Interface Properties

        int IUpdateTracker.RunningUpdateCount
        {
            get
            {
                return _runningUpdateCount;
            }

            set
            {
                _runningUpdateCount = value;
            }
        }

        #endregion

        #region Public Indexers

        public object this[int index]
        {
            get
            {
                return _view[index];
            }

            set
            {
                _view[index] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add(object item)
        {
            CheckReadOnly();
            _sourceList.Add(item);
        }

        public object AddNew()
        {
            CurrentAddItem = null;
            if (_itemType != null)
            {
                CurrentAddItem = Activator.CreateInstance(_itemType);
                if (CurrentAddItem != null)
                {
                    Add(CurrentAddItem);
                }
            }

            return CurrentAddItem;
        }

        public void CancelEdit()
        {
            var ieo = CurrentEditItem as IEditableObject;
            if (ieo != null)
            {
                ieo.CancelEdit();
            }

            CurrentEditItem = null;
        }

        public void CancelNew()
        {
            if (CurrentAddItem != null)
            {
                Remove(CurrentAddItem);
                CurrentAddItem = null;
            }
        }

        public void Clear()
        {
            CheckReadOnly();
            _sourceList.Clear();
        }

        public void CommitEdit()
        {
            if (CurrentEditItem != null)
            {
                // finish editing item
                var item = CurrentEditItem;
                var ieo = item as IEditableObject;
                if (ieo != null)
                {
                    ieo.EndEdit();
                }

                CurrentEditItem = null;

                // apply filter/sort after edits
                HandleItemChanged(item);
            }
        }

        public void CommitNew()
        {
            if (CurrentAddItem != null)
            {
                var item = CurrentAddItem;
                CurrentAddItem = null;
                HandleItemChanged(item);
            }
        }

        public bool Contains(object item)
        {
            return _view.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            _view.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay
        /// automatic refresh.
        /// </summary>
        public IDisposable DeferRefresh()
        {
            return new DeferNotifications(this, this);
        }

        public void EditItem(object item)
        {
            var ieo = item as IEditableObject;
            if (ieo != null && ieo != CurrentEditItem)
            {
                ieo.BeginEdit();
            }

            CurrentEditItem = item;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _view.GetEnumerator();
        }

        public int IndexOf(object item)
        {
            return _view.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            CheckReadOnly();
            if (_sort.Count > 0 || _filter != null)
            {
                throw new Exception("Cannot insert items into sorted or filtered views.");
            }

            _sourceList.Insert(index, item);
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            throw new NotSupportedException();
        }

        public bool MoveCurrentTo(object item)
        {
            return item == CurrentItem || MoveCurrentToIndex(IndexOf(item));
        }

        public bool MoveCurrentToFirst()
        {
            return MoveCurrentToIndex(0);
        }

        public bool MoveCurrentToLast()
        {
            return MoveCurrentToIndex(_view.Count - 1);
        }

        public bool MoveCurrentToNext()
        {
            return MoveCurrentToIndex(CurrentPosition + 1);
        }

        public bool MoveCurrentToPosition(int index)
        {
            return MoveCurrentToIndex(index);
        }

        public bool MoveCurrentToPrevious()
        {
            return MoveCurrentToIndex(CurrentPosition - 1);
        }

        /// <summary>
        /// Update the view from the current source, using the current filter and sort settings.
        /// </summary>
        public void Refresh()
        {
            HandleSourceChanged();
        }

        public bool Remove(object item)
        {
            CheckReadOnly();
            _sourceList.Remove(item);
            return true;
        }

        public void RemoveAt(int index)
        {
            Remove(_view[index]);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _view.GetEnumerator();
        }

        int IComparer<object>.Compare(object x, object y)
        {
            // get property descriptors (once)
            if (_sortProps.Count == 0)
            {
                var typeInfo = x.GetType().GetTypeInfo();
                foreach (var sd in _sort)
                {
                    _sortProps[sd.PropertyName] = typeInfo.GetDeclaredProperty(sd.PropertyName);
                }
            }

            // compare two items
            foreach (var sd in _sort)
            {
                var pi = _sortProps[sd.PropertyName];
                var cx = pi.GetValue(x) as IComparable;
                var cy = pi.GetValue(y) as IComparable;

                try
                {
                    var cmp = Equals(cx, cy) ? 0 : cx == null ? -1 : cy == null ? +1 : cx.CompareTo(cy);

                    if (cmp != 0)
                    {
                        return sd.Direction == ListSortDirection.Ascending ? +cmp : -cmp;
                    }
                }
                catch
                {
                    Debug.WriteLine("comparison failed...");
                }
            }

            return 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="CurrentChanged"/> event.
        /// </summary>
        protected virtual void OnCurrentChanged(object e)
        {
            if (_runningUpdateCount <= 0)
            {
                if (CurrentChanged != null)
                {
                    CurrentChanged(this, e);
                }

                OnPropertyChanged("CurrentItem");
            }
        }

        /// <summary>
        /// Raises the <see cref="CurrentChanging"/> event.
        /// </summary>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            if (_runningUpdateCount <= 0)
            {
                if (CurrentChanging != null)
                {
                    CurrentChanging(this, e);
                }
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Raises the <see cref="VectorChanged"/> event.
        /// </summary>
        protected virtual void OnVectorChanged(IVectorChangedEventArgs e)
        {
            if (IsAddingNew || IsEditingItem)
            {
                throw new NotSupportedException("Cannot change collection while adding or editing items.");
            }

            if (_runningUpdateCount <= 0)
            {
                if (VectorChanged != null)
                {
                    VectorChanged(this, e);
                }

                OnPropertyChanged("Count");
            }
        }

        private void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw new Exception("The source collection cannot be modified.");
            }
        }

        private Type GetItemType()
        {
            Type itemType = null;
            if (_source != null)
            {
                var type = _source.GetType();
                var args = type.GenericTypeArguments;
                if (args.Length == 1)
                {
                    itemType = args[0];
                }
                else if (_sourceList != null && _sourceList.Count > 0)
                {
                    var item = _sourceList[0];
                    itemType = item.GetType();
                }
            }

            return itemType;
        }

        private void HandleItemAdded(int index, object item)
        {
            // if the new item is filtered out of view, no work
            if (_filter != null && !_filter(item))
            {
                return;
            }

            // compute insert index
            if (_sort.Count > 0)
            {
                // sorted: insert at sort position
                _sortProps.Clear();
                index = _view.BinarySearch(item, this);
                if (index < 0)
                {
                    index = ~index;
                }
            }
            else if (_filter != null)
            {
                // if the source is not a list (e.g. enum), then do a full refresh
                if (_sourceList == null)
                {
                    HandleSourceChanged();
                    return;
                }

                // find insert index
                // count invisible items below the insertion point and
                // subtract from the number of items in the view
                // (counting from the bottom is more efficient for the
                // most common case which is appending to the source collection)
                var visibleBelowIndex = 0;
                for (int i = index; i < _sourceList.Count; i++)
                {
                    if (!_filter(_sourceList[i]))
                    {
                        visibleBelowIndex++;
                    }
                }

                index = _view.Count - visibleBelowIndex;
            }

            // add item to view
            _view.Insert(index, item);

            // keep selection on the same item
            if (index <= CurrentPosition)
            {
                CurrentPosition++;
            }

            // notify listeners
            var e = new VectorChangedEventArgs(CollectionChange.ItemInserted, index);
            OnVectorChanged(e);
        }

        private void HandleItemChanged(object item)
        {
            // apply filter/sort after edits
            var refresh = _filter != null && !_filter(item);

            if (_sort.Count > 0)
            {
                // find sorted index for this object
                _sortProps.Clear();
                var newIndex = _view.BinarySearch(item, this);
                if (newIndex < 0)
                {
                    newIndex = ~newIndex;
                }

                // item moved within the collection
                if (newIndex >= _view.Count || _view[newIndex] != item)
                {
                    refresh = true;
                }
            }

            if (refresh)
            {
                HandleSourceChanged();
            }
        }

        private void HandleItemRemoved(int index, object item)
        {
            // check if the item is in the view
            if (_filter != null && !_filter(item))
            {
                return;
            }

            // compute index into view
            if (index < 0 || index >= _view.Count || !Equals(_view[index], item))
            {
                index = _view.IndexOf(item);
            }

            if (index < 0)
            {
                return;
            }

            // remove item from view
            _view.RemoveAt(index);

            // keep selection on the same item
            if (index <= CurrentPosition)
            {
                CurrentPosition--;
            }

            // notify listeners
            var e = new VectorChangedEventArgs(CollectionChange.ItemRemoved, index);
            OnVectorChanged(e);
        }

        // update view after changes other than add/remove an item
        private void HandleSourceChanged()
        {
            // release sort property PropertyInfo dictionary
            _sortProps.Clear();

            // keep selection if possible
            var currentItem = CurrentItem;

            // re-create view
            _view.Clear();
            var ie = Source as IEnumerable;
            if (ie != null)
            {
                foreach (var item in ie)
                {
                    if (_filter == null || _filter(item))
                    {
                        if (_sort.Count > 0)
                        {
                            var index = _view.BinarySearch(item, this);
                            if (index < 0)
                            {
                                index = ~index;
                            }

                            _view.Insert(index, item);
                        }
                        else
                        {
                            _view.Add(item);
                        }
                    }
                }
            }

            // release sort property PropertyInfo dictionary
            _sortProps.Clear();

            // notify listeners
            OnVectorChanged(new VectorChangedEventArgs(CollectionChange.Reset));

            // restore selection if possible
            MoveCurrentTo(currentItem);
        }

        private bool MoveCurrentToIndex(int index)
        {
            // invalid?
            if (index < -1 || index >= _view.Count)
            {
                return false;
            }

            // no change?
            if (index == CurrentPosition)
            {
                return false;
            }

            // fire changing
            var e = new CurrentChangingEventArgs();
            OnCurrentChanging(e);
            if (e.Cancel)
            {
                return false;
            }

            // change and fire changed
            CurrentPosition = index;
            OnCurrentChanged(null);
            return true;
        }

        private void _sort_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_runningUpdateCount <= 0)
            {
                HandleSourceChanged();
            }
        }

        private void _sourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_runningUpdateCount <= 0)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count == 1)
                        {
                            HandleItemAdded(e.NewStartingIndex, e.NewItems[0]);
                        }
                        else
                        {
                            HandleSourceChanged();
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems.Count == 1)
                        {
                            HandleItemRemoved(e.OldStartingIndex, e.OldItems[0]);
                        }
                        else
                        {
                            HandleSourceChanged();
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        HandleSourceChanged();
                        break;
                    default:
                        throw new Exception("Unrecognized collection change notification: " + e.Action.ToString());
                }
            }
        }

        #endregion
    }
}