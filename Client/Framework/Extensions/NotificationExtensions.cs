namespace Subsonic8.Framework.Extensions
{
    using Client.Common.Models;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;

    internal static class NotificationExtensions
    {
        #region Public Methods and Operators

        public static async void ShowToast<T>(this T notificationServiceOwner, PlaylistItem model)
            where T : IToastNotificationCapable
        {
            await notificationServiceOwner.ToastNotificationService.Show(GetNotificationOptionsFromPlaylistItem(model));
        }

        public static async void UpdateTile<T>(this T notificationServiceOwner, PlaylistItem model)
            where T : ITileNotificationCapable
        {
            await notificationServiceOwner.TileNotificationService.Show(GetNotificationOptionsFromPlaylistItem(model));
        }

        #endregion

        #region Methods

        private static PlaybackNotificationOptions GetNotificationOptionsFromPlaylistItem(PlaylistItem model)
        {
            return new PlaybackNotificationOptions
                       {
                           ImageUrl = model.OriginalCoverArtUrl, 
                           Title = model.Title, 
                           Subtitle = model.Artist
                       };
        }

        #endregion
    }
}