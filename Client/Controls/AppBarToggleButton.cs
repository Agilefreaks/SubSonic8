using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Controls
{
    public class AppBarToggleButton : Button
    {
        private object _content;
        private Style _style;

        // TO USE IsChecked It has to be initialized LAST in XAML
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(AppBarToggleButton), new PropertyMetadata(false, (o, e) => (o as AppBarToggleButton).IsCheckedChanged()));

        public string CheckedContent
        {
            get { return (string)GetValue(CheckedContentProperty); }
            set { SetValue(CheckedContentProperty, value); }
        }

        public static readonly DependencyProperty CheckedContentProperty =
            DependencyProperty.Register("CheckedContent", typeof(string), typeof(AppBarToggleButton), null);

        public Style CheckedStyle
        {
            get { return (Style)GetValue(CheckedStyleProperty); }
            set { SetValue(CheckedStyleProperty, value); }
        }

        public static readonly DependencyProperty CheckedStyleProperty =
            DependencyProperty.Register("CheckedStyle", typeof(Style), typeof(AppBarToggleButton), null);

        public bool AutoToggle
        {
            get { return (bool)GetValue(AutoToggleProperty); }
            set { SetValue(AutoToggleProperty, value); }
        }

        public static readonly DependencyProperty AutoToggleProperty =
            DependencyProperty.Register("AutoToggle", typeof(bool), typeof(AppBarToggleButton), null);

        private void IsCheckedChanged()
        {
            if (IsChecked)
            {
                _content = Content;
                _style = Style;

                if (CheckedStyle == null) Content = CheckedContent;
                else Style = CheckedStyle;
            }
            else
            {
                if (CheckedStyle == null) Content = _content;
                else Style = _style;
            }
        }

        protected override void OnTapped(Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            if (AutoToggle) IsChecked = !IsChecked;
        }
    }
}
