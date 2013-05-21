namespace Subsonic8.BottomBar
{
    using Windows.UI.Xaml.Controls;

    public sealed partial class PlaylistBottomBarView
    {
        #region Constructors and Destructors

        public PlaylistBottomBarView()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public Grid GetLayoutRoot()
        {
            return LayoutRoot;
        }

        #endregion
    }
}