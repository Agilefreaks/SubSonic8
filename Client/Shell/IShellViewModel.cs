using System;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;
using Client.Common.Services.DataStructures.PlayerManagementService;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen, IBottomBarViewModelProvider, IErrorHandler, IPlayer,
        IHandle<ChangeBottomBarMessage>
    {
        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }

        IToastNotificationService NotificationService { get; set; }

        IDialogNotificationService DialogNotificationService { get; set; }

        IPlayerControls PlayerControls { get; set; }

        void SendSearchQueryMessage(string query);
    }
}