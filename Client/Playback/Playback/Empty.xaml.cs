using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Playback.Playback
{
    public sealed partial class Empty
    {
        public Empty()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
