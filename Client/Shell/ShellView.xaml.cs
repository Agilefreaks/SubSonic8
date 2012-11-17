using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Shell
{
    public sealed partial class ShellView
    {
        public Frame ShellFrame
        {
            get { return shellFrame; }
        }

        public ShellView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaControl.PlayPressed += MediaControl_PlayPressed;
            MediaControl.PausePressed += MediaControl_PausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControl_StopPressed;
        }

        private void MediaControl_PlayPressed(object sender, object e)
        {
        }

        private void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
        }

        private void MediaControl_PausePressed(object sender, object e)
        {
        }

        private void MediaControl_StopPressed(object sender, object e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
