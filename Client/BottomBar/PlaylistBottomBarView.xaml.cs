using Windows.UI.Xaml.Controls;

namespace Subsonic8.BottomBar
{
    public sealed partial class PlaylistBottomBarView
    {
        public PlaylistBottomBarView()
        {
            InitializeComponent();
        }

        public Grid GetLayoutRoot()
        {
            return LayoutRoot;
        }
    }
}