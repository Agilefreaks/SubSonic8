using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.Album;
using Subsonic8.MusicDirectory;
using Subsonic8.Playback;

namespace Subsonic8.Framework.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static void
            NavigateByEntityType(this INavigationService navigationService, ISubsonicModel subsonicModel)
        {
            switch (subsonicModel.Type)
            {
                case SubsonicModelTypeEnum.Song:
                    navigationService.NavigateToViewModel<PlaybackViewModel>(subsonicModel);
                    break;
                case SubsonicModelTypeEnum.Album:
                    navigationService.NavigateToViewModel<AlbumViewModel>(subsonicModel);
                    break;
                case SubsonicModelTypeEnum.MusicDirectory:
                    navigationService.NavigateToViewModel<MusicDirectoryViewModel>(subsonicModel);
                    break;
            }
        }
    }
}
