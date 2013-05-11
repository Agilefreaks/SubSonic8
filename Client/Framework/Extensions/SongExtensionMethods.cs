using Client.Common.Models;
using Client.Common.Services;

namespace Subsonic8.Framework.Extensions
{
    public static class SongExtensionMethods
    {
        public static Client.Common.Models.PlaylistItem AsPlaylistItem(this ISongModel songModel, ISubsonicService subsonicService)
        {
            var playlistItem = new Client.Common.Models.PlaylistItem
                {
                    Artist = songModel.Artist,
                    Title = songModel.Name,
                    Duration = songModel.Duration,
                    Uri = songModel.Type == SubsonicModelTypeEnum.Video
                              ? subsonicService.GetUriForVideoWithId(songModel.Id)
                              : subsonicService.GetUriForFileWithId(songModel.Id),
                    CoverArtUrl = subsonicService.GetCoverArtForId(songModel.CoverArt),
                    Type = songModel.Type == SubsonicModelTypeEnum.Video
                               ? PlaylistItemTypeEnum.Video
                               : PlaylistItemTypeEnum.Audio
                };


            return playlistItem;
        }

    }
}