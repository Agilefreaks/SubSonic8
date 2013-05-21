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
                typeof(object), 
                typeof(ScrollIntoViewBehavior), 
                new PropertyMetadata(null, ActiveItemProviderChangedCallback));

        #endregion

        #region Fields

        private IActiveItemProvider _provider;

        #endregion

        #region Public Properties

        public object ActiveItemProvider
        {
            get
            {
                return GetValue(ActiveItemProviderProperty);
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
            var provider = ActiveItemProvider as IActiveItemProvider;
            if (provider != null)
            {
                provider.PropertyChanged -= ActiveItemProviderOnPropertyChanged;
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

        private void ActiveItemProviderOnPropertyChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "ActiveItem")
            {
                return;
            }

            AssociatedObject.ScrollIntoView(_provider.ActiveItem);
        }

        private bool CanStartWatchingActiveItem()
        {
            return _provider != null && AssociatedObject.Items != null;
        }

        private void StartFollowing(INotifyPropertyChanged activeItemProvider)
        {
            _provider = ActiveItemProvider as IActiveItemProvider;
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