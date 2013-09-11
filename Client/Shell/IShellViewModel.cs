namespace Subsonic8.Shell
{
    using System;
    using Caliburn.Micro;
    using Client.Common.Results;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;

    public interface IShellViewModel : IViewAware,
                                       IScreen,
                                       IBottomBarViewModelProvider,
                                       IErrorHandler,
                                       IPlayer,
                                       IHandle<ChangeBottomBarMessage>
    {
        #region Public Properties

        IDialogNotificationService DialogNotificationService { get; set; }

        IToastNotificationService NotificationService { get; set; }

        IPlayerControls PlayerControls { get; set; }

        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }

        #endregion

        #region Public Methods and Operators

        void SendSearchQueryMessage(string query);

        #endregion
    }
}