using System.ComponentModel;
using Windows.UI.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.Behaviors
{
    public class ScrollIntoViewBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty ActiveItemProviderProperty = DependencyProperty.Register(
            "ActiveItemProvider", typeof(object),
            typeof(ScrollIntoViewBehavior),
            new PropertyMetadata(null, ActiveItemProviderChangedCallback));

        private IActiveItemProvider _provider;

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

        private void StartFollowing(INotifyPropertyChanged activeItemProvider)
        {
            _provider = ActiveItemProvider as IActiveItemProvider;
            if (!CanStartWatchingActiveItem()) return;
            activeItemProvider.PropertyChanged += ActiveItemProviderOnPropertyChanged;
        }

        private void StopFollowing(INotifyPropertyChanged oldActiveItemProvider)
        {
            oldActiveItemProvider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
        }

        private void ActiveItemProviderOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "ActiveItem") return;
            AssociatedObject.ScrollIntoView(_provider.ActiveItem);
        }

        private bool CanStartWatchingActiveItem()
        {
            return _provider != null && AssociatedObject.Items != null;
        }
    }
}
