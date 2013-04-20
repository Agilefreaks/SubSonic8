using System;
using System.Threading.Tasks;
using Subsonic8.Framework.Interfaces;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Shell
{
    public sealed partial class ShellView : IPlayerControls
    {
        public Action StopAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PauseAction { get; private set; }

        public Frame ShellFrame
        {
            get { return shellFrame; }
        }

        public ShellView()
        {
            InitializeComponent();
            StopAction = CallStop;
            PauseAction = CallPause;
            PlayAction = CallPlay;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void CallPlay()
        {
            await RunOnDispatcher(() => mediaElement.Play());
        }

        private async void CallPause()
        {
            await RunOnDispatcher(() => mediaElement.Pause());
        }

        private async void CallStop()
        {
            await RunOnDispatcher(() => mediaElement.Stop());
        }

        private async Task RunOnDispatcher(Action action)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => action());
        }
    }
}
