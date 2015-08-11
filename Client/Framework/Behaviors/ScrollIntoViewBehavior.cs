namespace Subsonic8.Framework.Behaviors
{
    using System.ComponentModel;
    using Windows.UI.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class ScrollIntoViewBehavior : Behavior<ListView>
    {
        #region Static Fields

        public static readonly DependencyProperty ActiveItemProviderProperty =
            DependencyProperty.Register(
                "ActiveItemProvider", 
                typeof(IActiveItemProvider), 
                typeof(ScrollIntoViewBehavior), 
                new PropertyMetadata(null, ActiveItemProviderChangedCallback));

        #endregion

        #region Public Properties

        public IActiveItemProvider ActiveItemProvider
        {
            get
            {
                return GetValue(ActiveItemProviderProperty) as IActiveItemProvider;
            }

            set
            {
                SetValue(ActiveItemProviderProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void Detach()
        {
            base.Detach();
            if (ActiveItemProvider != null)
            {
                ActiveItemProvider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
            }
        }

        public override void Attach(FrameworkElement frameworkElement)
        {
            base.Attach(frameworkElement);
            if (ActiveItemProvider != null)
            {
                AssociatedObject.ScrollIntoView(ActiveItemProvider.ActiveItem);
            }
        }

        #endregion

        #region Methods

        private static void ActiveItemProviderChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var scrollIntoViewBehavior = dependencyObject as ScrollIntoViewBehavior;
            if (scrollIntoViewBehavior == null)
            {
                return;
            }

            var oldActiveItemProvider = dependencyPropertyChangedEventArgs.OldValue as IActiveItemProvider;
            if (oldActiveItemProvider != null)
            {
                scrollIntoViewBehavior.StopFollowing(oldActiveItemProvider);
            }

            var activeItemProvider = dependencyPropertyChangedEventArgs.NewValue as IActiveItemProvider;
            if (activeItemProvider != null)
            {
                scrollIntoViewBehavior.StartFollowing(activeItemProvider);
            }
        }

        private void ActiveItemProviderOnPropertyChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "ActiveItem")
            {
                return;
            }

            AssociatedObject.ScrollIntoView(ActiveItemProvider.ActiveItem);
        }

        private bool CanStartWatchingActiveItem()
        {
            return ActiveItemProvider != null && AssociatedObject.Items != null;
        }

        private void StartFollowing(INotifyPropertyChanged activeItemProvider)
        {
            if (!CanStartWatchingActiveItem())
            {
                return;
            }

            activeItemProvider.PropertyChanged += ActiveItemProviderOnPropertyChanged;
        }

        private void StopFollowing(INotifyPropertyChanged oldActiveItemProvider)
        {
            oldActiveItemProvider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
        }

        #endregion
    }
}