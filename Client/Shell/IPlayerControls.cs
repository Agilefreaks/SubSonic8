using Windows.UI.Xaml;

namespace Subsonic8.Shell
{
    public interface IPlayerControls
    {
        event RoutedEventHandler PlayNextClicked;
        event RoutedEventHandler PlayPreviousClicked;
    }
}