using Client.Common.MugenExtensions;
using Subsonic8.BottomBar;
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
                    typeof (IShellViewModel),
                    typeof (IPlaybackViewModel),
                    typeof (IFullScreenVideoPlaybackViewModel),
                    typeof (IEmbededVideoPlaybackViewModel),
                    typeof (IDefaultBottomBarViewModel)
                });
        }
    }
}