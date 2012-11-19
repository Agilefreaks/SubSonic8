using System;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
            MediaControl.PlayPressed += MediaControlPlayPressed;
            MediaControl.PausePressed += MediaControlPausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControlPlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControlStopPressed;
        }

        private async void MediaControlPlayPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => mediaElement.Play());
        }

        private async void MediaControlPlayPauseTogglePressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                      () =>
                                          {
                                              if (mediaElement.CurrentState == MediaElementState.Paused)
                                              {
                                                  mediaElement.Play();
                                              }
                                              else
                                              {
                                                  mediaElement.Pause();
                                              }
                                          });
        }

        private async void MediaControlPausePressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => mediaElement.Pause());
        }

        private async void MediaControlStopPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => mediaElement.Stop());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
