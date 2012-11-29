using System.Collections.ObjectModel;
using System.Linq;
using Subsonic8.MenuItem;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Search
{
    /// <summary>
    /// Adds Multiple Selection behavior to ListViewBase
    /// This adds capabilities to set/get Multiple selection from Binding (ViewModel)
    /// </summary>
    public class MultipleSelectBehavior
    {
        #region SelectedItems Attached Property

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems",
            typeof(object),
            typeof(MultipleSelectBehavior),
            new PropertyMetadata(new ObservableCollection<MenuItemViewModel>(), SelectedItemsChange));

        public static void SetSelectedItems(DependencyObject obj, ObservableCollection<MenuItemViewModel> selectedItems)
        {
            obj.SetValue(SelectedItemsProperty, selectedItems);
        }

        public static ObservableCollection<MenuItemViewModel> GetSelectedItems(DependencyObject obj)
        {
            return (ObservableCollection<MenuItemViewModel>)obj.GetValue(SelectedItemsProperty);
        }

        private static void SelectedItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GridView)d).SelectionChanged += OnSelectionChanged;
            ((GridView)d).Unloaded += OnUnloaded;
        }

        private static void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ((GridView)sender).SelectionChanged -= OnSelectionChanged;
            ((GridView)sender).Unloaded -= OnUnloaded;
        }

        #endregion

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = GetSelectedItems((DependencyObject)sender);

            foreach (var item in e.RemovedItems.Where(selectedItems.Contains))
            {
                selectedItems.Remove((MenuItemViewModel) item);
            }

            foreach (var item in e.AddedItems.Where(item => !selectedItems.Contains(item)))
            {
                selectedItems.Add((MenuItemViewModel) item);
            }

            SetSelectedItems((DependencyObject)sender, selectedItems);
        }
    }
}