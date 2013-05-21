namespace Subsonic8.BottomBar
{
    using System;
    using Callisto.Controls;
    using Windows.UI.Xaml;

    public sealed partial class RenamePlaylistDialog
    {
        #region Constructors and Destructors

        public RenamePlaylistDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public Action<string> OnOkClick { get; set; }

        public string PlaylistName
        {
            get
            {
                return PlaylistNameTextBox.Text;
            }

            set
            {
                PlaylistNameTextBox.Text = value;
            }
        }

        #endregion

        #region Methods

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            if (OnOkClick == null)
            {
                return;
            }

            PlaylistNameTextBox.IsEnabled = false;
            OkButton.IsEnabled = false;
            OnOkClick(PlaylistNameTextBox.Text);
            var flyout = Parent as Flyout;
            if (flyout != null)
            {
                flyout.IsOpen = false;
            }
        }

        private void PlaylistNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PlaylistNameTextBox.SelectionStart = PlaylistNameTextBox.Text.Length;
        }

        #endregion
    }
}