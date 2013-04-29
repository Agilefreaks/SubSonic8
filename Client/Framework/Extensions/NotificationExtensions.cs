using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;

namespace Subsonic8.Framework.Extensions
{
    static class NotificationExtensions
    {
        public static async void ShowToast<T>(this T notificationServiceOwner, Client.Common.Models.PlaylistItem model)
            where T : IToastNotificationCapable
        {
            await notificationServiceOwner.ToastNotificationService.Show(GetNotificationOptionsFromPlaylistItem(model));
        }

        public static async void UpdateTile<T>(this T notificationServiceOwner, Client.Common.Models.PlaylistItem model)
            where T : ITileNotificationCapable
        {
            await notificationServiceOwner.TileNotificationService.Show(GetNotificationOptionsFromPlaylistItem(model));
        }

        private static PlaybackNotificationOptions GetNotificationOptionsFromPlaylistItem(Client.Common.Models.PlaylistItem model)
        {
            return new PlaybackNotificationOptions
                {
                    ImageUrl = model.OriginalCoverArtUrl,
                    Title = model.Title,
                    Subtitle = model.Artist
                };
        }
    }
}
