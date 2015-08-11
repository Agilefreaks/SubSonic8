namespace Subsonic8.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class StringToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, System.Type targetType, Object parameter, string language)
        {
            // Tip: The value to check is "value" argument. It is a generic object.
            // You will have to cast it to string then verify if it is null or empty.
            // Use "string.IsNullOrEmpty" method

            throw new System.NotImplementedException();
        }

        public Object ConvertBack(Object value, System.Type targetType, Object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
