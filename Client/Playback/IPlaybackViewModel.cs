namespace Subsonic8.Playback
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Subsonic8.Framework.Behaviors;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.ViewModel;

    public interface IPlaybackViewModel : IHandle<PlaylistStateChangedMessage>,
                                          IHandle<StartPlaybackMessage>,
                                          IHandle<PlayFailedMessage>,
                                          IViewModel,
                                          IToastNotificationCapable,
                                          IHaveState,
                                          IVisualStateAware,
                                          IActiveItemProvider
    {
        #region Public Properties

        string CoverArt { get; set; }

        bool IsPlaying { get; }

        PlaybackViewModelStateEnum State { get; }

        #endregion

        #region Public Methods and Operators

        void ClearPlaylist();

        Task SavePlaylist();

        #endregion
    }
}