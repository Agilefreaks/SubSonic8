using System;
using Client.Common.Services.DataStructures.PlayerManagementService;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public interface IVidePlaybackViewModel : IViewModel, IToastNotificationCapable, IPlayer
    {
        event EventHandler<PlaybackStateEventArgs> FullScreenChanged;
    }
}