using System;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Subsonic8.Shell
{
    public sealed partial class ShellView : IPlayerControls
    {
        public event RoutedEventHandler PlayNextClicked;
        
        public event RoutedEventHandler PlayPreviousClicked;

        public Action PlayPause { get; private set; }

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
            PlayPause = CallPlayPause;
            MediaControl.PlayPressed += MediaControlPlayPressed;
            MediaControl.PausePressed += MediaControlPausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControlPlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControlStopPressed;
            MediaControl.NextTrackPressed += PlayNextTrackPressed;
            MediaControl.PreviousTrackPressed += PlayPreviousTrackPressed;
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

        public void PlayNextTrackPressed(object sender, object e)
        {
            PlayNextClicked(mediaElement, (RoutedEventArgs) e);
        }

        public void PlayPreviousTrackPressed(object sender, object e)
        {
            PlayPreviousClicked(mediaElement, (RoutedEventArgs)e);
        }

        private void CallPlayPause()
        {
            MediaControlPlayPauseTogglePressed(null, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
