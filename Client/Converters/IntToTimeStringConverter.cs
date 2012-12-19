using System;
using Windows.UI.Xaml.Data;

namespace Subsonic8.Converters
{
    public class IntToTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = "";
            if (value is int)
            {
                var intValue = (int)value;
                var timeSpan = new TimeSpan(0, 0, intValue);

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
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
