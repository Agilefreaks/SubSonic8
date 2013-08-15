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

        public async void Pause()
        {
            await RunOnDispatcher(() => mediaElement.Pause());
        }

        public async void Play()
        {
            await RunOnDispatcher(() => mediaElement.Play());
        }

        public async void Stop()
        {
            await RunOnDispatcher(() => mediaElement.Stop());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async Task RunOnDispatcher(Action action)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        #endregion
    }
}