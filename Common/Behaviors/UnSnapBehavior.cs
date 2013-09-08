namespace Common.Behaviors
{
    using Windows.UI.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class UnsnapBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += AssociatedObjectOnClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= AssociatedObjectOnClick;
        }

        private static void AssociatedObjectOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();
        }
    }
}