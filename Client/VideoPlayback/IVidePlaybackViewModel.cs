using System;
using Client.Common.Services.DataStructures.PlayerManagementService;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    // ReSharper disable PossibleInterfaceMemberAmbiguity
    public interface IVidePlaybackViewModel : IPlaybackControlsViewModel, IViewModel, IToastNotificationCapable, IPlayer
    // ReSharper restore PossibleInterfaceMemberAmbiguity
    {
        event EventHandler<PlaybackStateEventArgs> FullScreenChanged;

        TimeSpan StartTime { get; set; }

        TimeSpan EndTime { get; set; }

        void OnFullScreenChanged(MediaPlayer mediaPlayer);
    }
}