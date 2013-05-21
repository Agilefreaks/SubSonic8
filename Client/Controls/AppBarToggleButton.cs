namespace Subsonic8.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    public class AppBarToggleButton : Button
    {
        #region Static Fields

        public static readonly DependencyProperty AutoToggleProperty = DependencyProperty.Register(
            "AutoToggle", typeof(bool), typeof(AppBarToggleButton), null);

        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register(
            "CheckedContent", typeof(string), typeof(AppBarToggleButton), null);

        public static readonly DependencyProperty CheckedStyleProperty = DependencyProperty.Register(
            "CheckedStyle", typeof(Style), typeof(AppBarToggleButton), null);

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", 
            typeof(bool), 
            typeof(AppBarToggleButton), 
            new PropertyMetadata(false, (o, e) => ((AppBarToggleButton)o).IsCheckedChanged()));

        #endregion

        #region Fields

        private object _content;

        private Style _style;

        #endregion

        #region Public Properties

        public bool AutoToggle
        {
            get
            {
                return (bool)GetValue(AutoToggleProperty);
            }

            set
            {
                SetValue(AutoToggleProperty, value);
            }
        }

        // TO USE IsChecked It has to be initialized LAST in XAML
        public string CheckedContent
        {
            get
            {
                return (string)GetValue(CheckedContentProperty);
            }

            set
            {
                SetValue(CheckedContentProperty, value);
            }
        }

        public Style CheckedStyle
        {
            get
            {
                return (Style)GetValue(CheckedStyleProperty);
            }

            set
            {
                SetValue(CheckedStyleProperty, value);
            }
        }

        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }

            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        #endregion

        #region Methods

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            if (AutoToggle)
            {
                IsChecked = !IsChecked;
            }
        }

        private void IsCheckedChanged()
        {
            if (IsChecked)
            {
                _content = Content;
                _style = Style;

                if (CheckedStyle == null)
                {
                    Content = CheckedContent;
                }
                else
                {
                    Style = CheckedStyle;
                }
            }
            else
            {
                if (CheckedStyle == null)
                {
                    Content = _content;
                }
                else
                {
                    Style = _style;
                }
            }
        }

        #endregion
    }
}