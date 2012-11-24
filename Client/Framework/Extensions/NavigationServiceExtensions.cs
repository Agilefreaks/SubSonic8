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
            NavigateByEntityType(this INavigationService navigationService, INavigableEntity navigableEntity)
        {
            switch (navigableEntity.Type)
            {
                case NavigableTypeEnum.Song:
                    navigationService.NavigateToViewModel<PlaybackViewModel>(navigableEntity);
                    break;
                case NavigableTypeEnum.Album:
                    navigationService.NavigateToViewModel<AlbumViewModel>(navigableEntity);
                    break;
                case NavigableTypeEnum.MusicDirectory:
                    navigationService.NavigateToViewModel<MusicDirectoryViewModel>(navigableEntity);
                    break;
            }
        }
    }
}
