﻿using System;
using Client.Common.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Subsonic8.Converters
{
    class PlayingStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = Visibility.Collapsed;
            var stringParameter = parameter as string;
            if (value.GetType() == typeof(PlaylistItemState) && parameter is string)
            {
                result = stringParameter == value.ToString() ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
