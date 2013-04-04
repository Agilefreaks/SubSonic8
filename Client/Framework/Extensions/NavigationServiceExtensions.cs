using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.Album;
using Subsonic8.Artist;
using Subsonic8.MusicDirectory;
using Subsonic8.Playback;

namespace Subsonic8.Framework.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static void NavigateByModelType<T>(this T navigationService, ISubsonicModel subsonicModel)
            where T : INavigationService
        {
            var id = subsonicModel.Id;
            switch (subsonicModel.Type)
            {
                case SubsonicModelTypeEnum.Song:
                case SubsonicModelTypeEnum.Video:
                    navigationService.NavigateToViewModel<PlaybackViewModel>(id);
                    break;
                case SubsonicModelTypeEnum.Album:
                    navigationService.NavigateToViewModel<AlbumViewModel>(id);
                    break;
                case SubsonicModelTypeEnum.MusicDirectory:
                    navigationService.NavigateToViewModel<MusicDirectoryViewModel>(id);
                    break;
                case SubsonicModelTypeEnum.Artist:
                    navigationService.NavigateToViewModel<ArtistViewModel>(id);
                    break;
            }
        }
    }
}
