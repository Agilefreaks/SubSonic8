using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Windows.UI.Xaml;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen, IBottomBarViewModelProvider, IErrorHandler, IHandle<StartAudioPlaybackMessage>, IHandle<StopAudioPlaybackMessage>,
        IHandle<ResumePlaybackMessage>, IHandle<PausePlaybackMessage>, IHandle<ChangeBottomBarMessage>
    {
        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }

        IToastNotificationService NotificationService { get; set; }

        IDialogNotificationService DialogNotificationService { get; set; }

        IPlayerControls PlayerControls { get; set; }

        void PlayNext(object sender, RoutedEventArgs routedEventArgs);

        void PlayPrevious(object sender, RoutedEventArgs routedEventArgs);

        void PlayPause();

        void Stop();

        void SendSearchQueryMessage(string query);
    }
}