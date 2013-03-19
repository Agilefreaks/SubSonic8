using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;

namespace Subsonic8.Framework.Extensions
{
    static class ToastNotificationExtensions
    {
        public static async void ShowToast<T>(this T notificationServiceOwner, Client.Common.Models.PlaylistItem model)
            where T : IToastNotificationCapable
        {
            await notificationServiceOwner.ToastNotificationService.Show(new ToastNotificationOptions
                {
                    ImageUrl = model.CoverArtUrl,
                    Title = model.Title,
                    Subtitle = model.Artist
                });
        }
    }
}
