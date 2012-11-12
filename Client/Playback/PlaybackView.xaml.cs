using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Playback
{
    public sealed partial class PlaybackView
    {
        public PlaybackView()
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
