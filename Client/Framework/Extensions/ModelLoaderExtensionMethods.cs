namespace Subsonic8.Framework.Extensions
{
    using System.Threading.Tasks;
    using Client.Common.Models;
    using Client.Common.Services;
    using Subsonic8.Framework.ViewModel;

    public static class ModelLoaderExtensionMethods
    {
        #region Public Methods and Operators

        public static async Task<PlaylistItem> LoadPlaylistItemFromSong(this ISongLoader modelLoader, IId model)
        {
            PlaylistItem playlistItem = null;
            if (model != null)
            {
                await
                    modelLoader.SubsonicService.GetSong(model.Id)
                               .WithErrorHandler(modelLoader.ErrorHandler)
                               .OnSuccess(
                                   result =>
                                   playlistItem = CreatePlaylistItemFromSong(result, modelLoader.SubsonicService))
                               .Execute();
            }

            return playlistItem;
        }

        #endregion

        #region Methods

        private static PlaylistItem CreatePlaylistItemFromSong(ISongModel result, ISubsonicService subsonicService)
        {
            var playlistItem = result.AsPlaylistItem(subsonicService);

            return playlistItem;
        }

        #endregion
    }
}