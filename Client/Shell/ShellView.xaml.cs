namespace Subsonic8.Shell
{
    using System;
    using System.Threading.Tasks;
    using Subsonic8.Framework.Interfaces;
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ShellView : IPlayerControls
    {
        #region Constructors and Destructors

        public ShellView()
        {
            InitializeComponent();
            StopAction = CallStop;
            PauseAction = CallPause;
            PlayAction = CallPlay;
        }

        #endregion

        #region Public Properties

        public Action PauseAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Frame ShellFrame
        {
            get
            {
                return shellFrame;
            }
        }

        public Action StopAction { get; private set; }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void CallPause()
        {
            await RunOnDispatcher(() => mediaElement.Pause());
        }

        private async void CallPlay()
        {
            await RunOnDispatcher(() => mediaElement.Play());
        }

        private async void CallStop()
        {
            await RunOnDispatcher(() => mediaElement.Stop());
        }

        private async Task RunOnDispatcher(Action action)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        #endregion
    }
}