using System;
using System.Threading.Tasks;
using Subsonic8.Framework.Interfaces;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Shell
{
    public sealed partial class ShellView : IPlayerControls
    {
        public Action PlayPause { get; private set; }

        public Action Stop { get; private set; }

        public Action Play { get; private set; }

        public Action Pause { get; private set; }

        public Frame ShellFrame
        {
            get { return shellFrame; }
        }

        public ShellView()
        {
            InitializeComponent();
            PlayPause = CallPlayPause;
            Stop = CallStop;
            Pause = CallPause;
            Play = CallPlay;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void CallPlayPause()
        {
            Action action;
            switch (mediaElement.CurrentState)
            {
                case MediaElementState.Stopped:
                case MediaElementState.Paused:
                    action = mediaElement.Play;
                    break;
                default:
                    action = mediaElement.Pause;
                    break;
            }

            await RunOnDispatcher(action);
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
