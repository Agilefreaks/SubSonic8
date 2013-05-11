using System.Threading.Tasks;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Framework.Extensions
{
    public static class ModelLoaderExtensionMethods
    {
        public static async Task<Client.Common.Models.PlaylistItem> LoadSong(this ISongLoader modelLoader, IId model)
        {
            Client.Common.Models.PlaylistItem playlistItem = null;
            if (model != null)
            {
                await modelLoader.SubsonicService.GetSong(model.Id)
                                 .WithErrorHandler(modelLoader)
                                 .OnSuccess(
                                     result =>
                                     playlistItem = CreatePlaylistItemFromSong(result, modelLoader.SubsonicService))
                                 .Execute();
            }

            return playlistItem;
        }

        private static Client.Common.Models.PlaylistItem CreatePlaylistItemFromSong(Song result, ISubsonicService subsonicService)
        {
            var playlistItem = result.AsPlaylistItem(subsonicService);

            return playlistItem;
        }
    }
}