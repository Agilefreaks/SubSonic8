﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework
{
    public class MultipleSelectBehavior
    {
        #region SelectedItems Attached Property

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems",
            typeof(object),
            typeof(MultipleSelectBehavior),
            new PropertyMetadata(new ObservableCollection<object>(), AttchedPropertyChanged));

        public static void SetSelectedItems(DependencyObject obj, ObservableCollection<object> selectedItems)
        {
            obj.SetValue(SelectedItemsProperty, selectedItems);
        }

        public static ObservableCollection<object> GetSelectedItems(DependencyObject obj)
        {
            return (ObservableCollection<object>)obj.GetValue(SelectedItemsProperty);
        }

        private static void AttchedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var observableCollection = eventArgs.NewValue as INotifyCollectionChanged;
            var gridView = (ListViewBase)dependencyObject;
            DoInitialSync(eventArgs.NewValue as IList, gridView.Items, gridView.SelectedItems);

            var collectionChangedEventHandler = SetupCollectionChangedEventHandler(dependencyObject, observableCollection);
            gridView.SelectionChanged += OnGridSelectionChanged;
            SetupOnUnloadedHandler(dependencyObject, observableCollection, collectionChangedEventHandler);
        }

        #endregion

        private static void SetupOnUnloadedHandler(DependencyObject dependencyObject, INotifyCollectionChanged observableCollection,
                                                   NotifyCollectionChangedEventHandler sourceChangedHandler)
        {
            RoutedEventHandler unloadedEventHandler = null;
            unloadedEventHandler = (sender, args) =>
                                       {
                                           observableCollection.CollectionChanged -= sourceChangedHandler;
                                           ((ListViewBase)sender).SelectionChanged -= OnGridSelectionChanged;
                                           ((ListViewBase)sender).Unloaded -= unloadedEventHandler;
                                       };
            ((ListViewBase)dependencyObject).Unloaded += unloadedEventHandler;
        }

        private static NotifyCollectionChangedEventHandler SetupCollectionChangedEventHandler(DependencyObject dependencyObject,
                                                                                          INotifyCollectionChanged observableCollection)
        {
            NotifyCollectionChangedEventHandler sourceChangedHandler =
                (s, ev) => SelectedItemsCollectionChanged(dependencyObject, ev);
            observableCollection.CollectionChanged += sourceChangedHandler;

            return sourceChangedHandler;
        }

        private static void DoInitialSync(IList sourceList, ICollection<object> items, ICollection<object> selectedItems)
        {
            var toRemove = sourceList.Cast<object>().Where(item => !items.Contains(item)).ToList();
            foreach (var item in toRemove)
            {
                sourceList.Remove(item);
            }

            foreach (var item in sourceList)
            {
                selectedItems.Add(item);
            }
        }

        private static void SelectedItemsCollectionChanged(DependencyObject dependencyObject, NotifyCollectionChangedEventArgs eventArgs)
        {
            var oldItems = eventArgs.OldItems ?? new List<object>();
            var newItems = eventArgs.NewItems ?? new List<object>();
            var gridSelectedItemsList = ((ListViewBase)dependencyObject).SelectedItems;
            UpdateCollection(gridSelectedItemsList, oldItems, newItems);
        }

        private static void OnGridSelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            var selectedItemsCollection = GetSelectedItems((DependencyObject)sender);
            UpdateCollection(selectedItemsCollection, eventArgs.RemovedItems, eventArgs.AddedItems);
        }

        private static void UpdateCollection(ICollection<object> collection, IEnumerable toRemove, IEnumerable toAdd)
        {
            foreach (var item in toRemove)
            {
                collection.Remove(item);
            }

            foreach (var item in toAdd)
            {
                collection.Add(item);
            }
        }
    }
}