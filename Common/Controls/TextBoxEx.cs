/*
 * Code taken from following article:
 * Title:   [Windows 8] Update TextBox’s binding on TextChanged 
 * Author:  Benjamin Roux
 * Url:     http://weblogs.asp.net/broux/archive/2012/07/03/windows-8-update-textbox-s-binding-on-textchanged.aspx
 */
namespace Common.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public static class TextBoxEx
    {
        public static readonly DependencyProperty RealTimeTextProperty =
    DependencyProperty.RegisterAttached("RealTimeText", typeof(string), typeof(TextBoxEx), null);

        public static readonly DependencyProperty IsAutoUpdateProperty =
    DependencyProperty.RegisterAttached("IsAutoUpdate", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false, OnIsAutoUpdateChanged));

        public static string GetRealTimeText(TextBox obj)
        {
            return (string)obj.GetValue(RealTimeTextProperty);
        }

        public static void SetRealTimeText(TextBox obj, string value)
        {
            obj.SetValue(RealTimeTextProperty, value);
        }

        public static bool GetIsAutoUpdate(TextBox obj)
        {
            return (bool)obj.GetValue(IsAutoUpdateProperty);
        }

        public static void SetIsAutoUpdate(TextBox obj, bool value)
        {
            obj.SetValue(IsAutoUpdateProperty, value);
        }

        private static void OnIsAutoUpdateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;
            var textbox = (TextBox)sender;

            if (value)
            {
                textbox.TextChanged += TextboxOnTextChanged;
            }
            else
            {
                textbox.TextChanged -= TextboxOnTextChanged;
            }
        }

        private static void TextboxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var textBox = (TextBox)sender;
            textBox.SetValue(RealTimeTextProperty, textBox.Text);
        }
    }
}