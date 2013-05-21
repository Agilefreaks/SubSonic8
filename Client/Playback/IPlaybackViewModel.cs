namespace Subsonic8.Playback
{
    using System;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Subsonic8.Framework.Behaviors;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.ViewModel;

    public interface IPlaybackViewModel : IHandle<PlaylistStateChangedMessage>, 
                                          IHandle<StartPlaybackMessage>, 
                                          IViewModel, 
                                          IToastNotificationCapable, 
                                          IHaveState, 
                                          IActiveItemProvider
    {
        #region Public Properties

        string CoverArt { get; set; }

        bool IsPlaying { get; }

        int? Parameter { set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; }

        #endregion

        #region Public Methods and Operators

        void ClearPlaylist();

        void SavePlaylist();

        #endregion
    }
}