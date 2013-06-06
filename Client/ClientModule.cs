namespace Subsonic8
{
    using System;
    using Caliburn.Micro;
    using global::Common.MugenExtensions;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Playback;
    using Subsonic8.Playlists;
    using Subsonic8.Shell;
    using Subsonic8.VideoPlayback;

    public class ClientModule : MugenModuleWithAutoDiscoveryBase
    {
        #region Methods

        protected override void PrepareForLoad()
        {
            Convetions.AddRange(
                new MugenConvetion[] { new ServiceConvention(Injector), new ViewModelConvention(Injector) });

            Singletons.AddRange(
                new[]
                    {
                        new Tuple<Type[], Type>(new[] { typeof(IEventAggregator) }, typeof(EventAggregator)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IShellViewModel), typeof(ShellViewModel) }, typeof(ShellViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IPlaybackViewModel), typeof(PlaybackViewModel) }, typeof(PlaybackViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IFullScreenVideoPlaybackViewModel) }, typeof(FullScreenVideoPlaybackViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IEmbededVideoPlaybackViewModel) }, typeof(EmbededVideoPlaybackViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IDefaultBottomBarViewModel) }, typeof(DefaultBottomBarViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IPlaybackBottomBarViewModel) }, typeof(PlaybackBottomBarViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IPlaylistBottomBarViewModel) }, typeof(PlaylistBottomBarViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(IManagePlaylistsViewModel), typeof(ManagePlaylistsViewModel) }, 
                            typeof(ManagePlaylistsViewModel)), 
                        new Tuple<Type[], Type>(
                            new[] { typeof(ISavePlaylistViewModel), typeof(SavePlaylistViewModel) }, 
                            typeof(SavePlaylistViewModel)), 
                        new Tuple<Type[], Type>(new[] { typeof(ISettingsHelper) }, typeof(SettingsHelper)), 
                        new Tuple<Type[], Type>(new[] { typeof(INotificationsHelper) }, typeof(NotificationsHelper))
                    });
        }

        #endregion
    }
}