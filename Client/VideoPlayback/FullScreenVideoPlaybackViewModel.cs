namespace Subsonic8.VideoPlayback
{
    public class FullScreenVideoPlaybackViewModel : VideoPlaybackViewModel, IFullScreenVideoPlaybackViewModel
    {
        protected override void OnStartingPlayback()
        {
            base.OnStartingPlayback();
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();
        }
    }
}