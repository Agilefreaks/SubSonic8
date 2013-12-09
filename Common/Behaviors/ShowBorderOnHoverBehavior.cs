namespace Common.Behaviors
{
    using Windows.UI;
    using Windows.UI.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    public class ShowBorderOnHoverBehavior : Behavior<Border>
    {
        private readonly SolidColorBrush _solidColorBrush;
        private Brush _previousBrush;
        private Thickness _previousThickness;

        public ShowBorderOnHoverBehavior()
        {
            _solidColorBrush = new SolidColorBrush(Color.FromArgb(0xf0, 0x2c, 0x25, 0x25));
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerEntered += AssociatedObjectOnPointerEntered;
            AssociatedObject.PointerExited += AssociatedObjectOnPointerExited;
            _previousBrush = AssociatedObject.BorderBrush;
            _previousThickness = AssociatedObject.BorderThickness;
            MakeBorderInvisible();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerEntered -= AssociatedObjectOnPointerEntered;
            AssociatedObject.PointerExited -= AssociatedObjectOnPointerExited;
        }

        private void AssociatedObjectOnPointerEntered(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            MakeBorderVisible();
        }

        private void AssociatedObjectOnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            MakeBorderInvisible();
        }

        private void MakeBorderInvisible()
        {
            AssociatedObject.BorderBrush = _previousBrush;
            AssociatedObject.BorderThickness = _previousThickness;
            AssociatedObject.Padding = new Thickness(5);
        }

        private void MakeBorderVisible()
        {
            _previousBrush = AssociatedObject.BorderBrush;
            _previousThickness = AssociatedObject.BorderThickness;
            AssociatedObject.BorderBrush = _solidColorBrush;
            AssociatedObject.BorderThickness = new Thickness(5, 5, 5, 5);
            AssociatedObject.Padding = new Thickness(0);
        }
    }
}
