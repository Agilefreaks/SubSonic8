namespace Subsonic8.Converters
{
    using System;
    using Subsonic8.Playback;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class PlaybackStateToVisibilityConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = Visibility.Collapsed;
            var stringParameter = parameter as string;
            if (value.GetType() == typeof(PlaybackViewModelStateEnum) && parameter is string)
            {
                result = stringParameter.Contains(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
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