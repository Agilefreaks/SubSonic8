namespace Subsonic8.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class IntToTimeStringConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result;
            var intValue = System.Convert.ToInt32(value);
            var timeSpan = TimeSpan.FromSeconds(intValue);

            if (timeSpan.Hours == 0)
            {
                result = timeSpan.ToString("mm\\:ss");
            }
            else if (timeSpan.Hours < 10)
            {
                result = timeSpan.ToString("h\\:mm\\:ss");
            }
            else
            {
                result = timeSpan.ToString("hh\\:mm\\:ss");
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}