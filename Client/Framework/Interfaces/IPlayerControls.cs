using System;
using Windows.UI.Xaml;

namespace Subsonic8.Framework.Interfaces
{
    public interface IPlayerControls
    {
        event RoutedEventHandler PlayNextClicked;

        event RoutedEventHandler PlayPreviousClicked;

        Action PlayPause { get; }

        Action Stop { get; }
    }
}