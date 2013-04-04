using System;
using Client.Common.MugenExtensions;
using Subsonic8.BottomBar;
using Subsonic8.Framework;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Playback;
using Subsonic8.Shell;
using Subsonic8.VideoPlayback;

namespace Subsonic8
{
    public class ClientModule : MugenModuleWithAutoDiscoveryBase
    {
        protected override void PrepareForLoad()
        {
            Convetions.AddRange(new MugenConvetion[]
                {
                    new ServiceConvention(Injector),
                    new ViewModelConvention(Injector)
                });

            Singletons.AddRange(new[]
                {
                    new Tuple<Type[], Type>(new[] { typeof(IShellViewModel), typeof(ShellViewModel) }, typeof(ShellViewModel)),
                    new Tuple<Type[], Type>(new[] { typeof(IPlaybackViewModel), typeof(PlaybackViewModel) }, typeof(PlaybackViewModel)),
                    new Tuple<Type[], Type>(new[] { typeof(IFullScreenVideoPlaybackViewModel), typeof(FullScreenVideoPlaybackViewModel) }, typeof(FullScreenVideoPlaybackViewModel)), 
                    new Tuple<Type[], Type>(new[] { typeof(IEmbededVideoPlaybackViewModel) }, typeof(EmbededVideoPlaybackViewModel)), 
                    new Tuple<Type[], Type>(new[] { typeof(IDefaultBottomBarViewModel) }, typeof(DefaultBottomBarViewModel)),
                    new Tuple<Type[], Type>(new[] { typeof(ISettingsHelper) }, typeof(SettingsHelper))
                });
        }
    }
}