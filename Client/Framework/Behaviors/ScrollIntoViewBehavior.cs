using System.ComponentModel;
using Windows.UI.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.Behaviors
{
    public class ScrollIntoViewBehavior : Behavior<ListView>
    {
        private const int SelectedItemOffset = 8;

        public static readonly DependencyProperty ActiveItemProviderProperty = DependencyProperty.Register(
            "ActiveItemProvider", typeof(object),
            typeof(ScrollIntoViewBehavior),
            new PropertyMetadata(null, ActiveItemProviderChangedCallback));

        private IActiveItemProvider _provider;
        private ScrollViewer _scrollViewer;
        private ItemCollection _itemsCollection;

        public object ActiveItemProvider
        {
            get { return GetValue(ActiveItemProviderProperty); }
            set { SetValue(ActiveItemProviderProperty, value); }
        }

        public override void Detach()
        {
            base.Detach();
            var provider = ActiveItemProvider as IActiveItemProvider;
            if (provider != null)
            {
                provider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
            }
        }

        private static void ActiveItemProviderChangedCallback(DependencyObject dependencyObject,
                                                              DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var scrollIntoViewBehavior = dependencyObject as ScrollIntoViewBehavior;
            if (scrollIntoViewBehavior == null) return;
            if (dependencyPropertyChangedEventArgs.OldValue != null)
            {
                var oldActiveItemProvider = dependencyPropertyChangedEventArgs.OldValue as IActiveItemProvider;
                if (oldActiveItemProvider != null)
                {
                    scrollIntoViewBehavior.StopFollowing(oldActiveItemProvider);
                }
            }

            if (dependencyPropertyChangedEventArgs.NewValue != null)
            {
                var activeItemProvider = dependencyPropertyChangedEventArgs.NewValue as IActiveItemProvider;
                if (activeItemProvider != null)
                {
                    scrollIntoViewBehavior.StartFollowing(activeItemProvider);
                }
            }
        }


        private static bool LowerThanViewPort(ScrollViewer scrollViewer, double targetLocation)
        {
            return (scrollViewer.VerticalOffset + scrollViewer.ActualHeight < targetLocation);
        }

        private static bool HigherThanViewPort(ScrollViewer scrollViewer, double targetLocation)
        {
            return scrollViewer.VerticalOffset >= targetLocation;
        }

        private void StartFollowing(INotifyPropertyChanged activeItemProvider)
        {
            _provider = ActiveItemProvider as IActiveItemProvider;
            _scrollViewer = AssociatedObject.Parent as ScrollViewer;
            if (!CanStartWatchingActiveItem()) return;
            _itemsCollection = AssociatedObject.Items;
            activeItemProvider.PropertyChanged += ActiveItemProviderOnPropertyChanged;
        }

        private void StopFollowing(INotifyPropertyChanged oldActiveItemProvider)
        {
            oldActiveItemProvider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
        }

        private void ActiveItemProviderOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "ActiveItem") return;
            var verticalOffset = GetActiveItemVerticalOffset();
            if (NotInViewPort(verticalOffset))
            {
                _scrollViewer.ScrollToVerticalOffset(verticalOffset);
            }
        }

        private double GetActiveItemVerticalOffset()
        {
            var indexOfActiveItem = _itemsCollection.IndexOf(_provider.ActiveItem);
            return indexOfActiveItem * ItemSize() - SelectedItemOffset;
        }

        private bool CanStartWatchingActiveItem()
        {
            return _provider != null && _scrollViewer != null && AssociatedObject.Items != null;
        }

        private double ItemSize()
        {
            return AssociatedObject.ActualHeight / _itemsCollection.Count;
        }

        private bool NotInViewPort(double targetLocation)
        {
            return HigherThanViewPort(_scrollViewer, targetLocation) || LowerThanViewPort(_scrollViewer, targetLocation);
        }
    }
}
