﻿namespace Subsonic8.Shell
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ShellView
    {
        #region Constructors and Destructors

        public ShellView()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public Frame ShellFrame
        {
            get
            {
                return shellFrame;
            }
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion
    }
}