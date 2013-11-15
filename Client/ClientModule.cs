namespace Subsonic8
{
    using Caliburn.Micro;
    using global::Common.MugenExtensions;
    using BottomBar;
    using ErrorDialog;
    using Framework;
    using Framework.Interfaces;
    using Framework.Services;
    using Playback;
    using Playlists;
    using Shell;
    using VideoPlayback;
    using SubLastFm;

    public class ClientModule : MugenModuleWithAutoDiscoveryBase
    {
        #region Methods

        protected override void PrepareForLoad()
        {
            Conventions.AddRange(new MugenConvetion[] { new ServiceConvention(Injector), new ViewModelConvention(Injector) });
            Singletons.Add<IEventAggregator, EventAggregator>();
            Singletons.Add<IErrorDialogViewModel, ErrorDialogViewModel>(true);
            Singletons.Add<IShellViewModel, ShellViewModel>(true);
            Singletons.Add<IPlaybackViewModel, PlaybackViewModel>(true);
            Singletons.Add<IFullScreenVideoPlaybackViewModel, FullScreenVideoPlaybackViewModel>();
            Singletons.Add<IEmbeddedVideoPlaybackViewModel, EmbeddedVideoPlaybackViewModel>();
            Singletons.Add<ISnappedVideoPlaybackViewModel, SnappedVideoPlaybackViewModel>();
            Singletons.Add<IDefaultBottomBarViewModel, DefaultBottomBarViewModel>();
            Singletons.Add<IPlaybackBottomBarViewModel, PlaybackBottomBarViewModel>();
            Singletons.Add<IPlaylistBottomBarViewModel, PlaylistBottomBarViewModel>();
            Singletons.Add<IManagePlaylistsViewModel, ManagePlaylistsViewModel>(true);
            Singletons.Add<ISavePlaylistViewModel, SavePlaylistViewModel>(true);
            Singletons.Add<ISettingsHelper, SettingsHelper>();
            Singletons.Add<INotificationsHelper, NotificationsHelper>();
            Singletons.Add<IConfigurationProvider, LastFmConfigurationProvider>();
            Singletons.Add<SubEchoNest.IConfigurationProvider, EchoNestConfigurationProvider>();
        }

        #endregion
    }
}