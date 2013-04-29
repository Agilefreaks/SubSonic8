using System;
using System.Threading.Tasks;
using NotificationsExtensions.TileContent;
using Windows.UI.Notifications;

namespace Subsonic8.Framework.Services
{
    public class TileNotificationService : ITileNotificationService
    {
        private readonly TileUpdater _tileUpdater;

        public TileNotificationService()
        {
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
        }

        public Task Show(PlaybackNotificationOptions options)
        {
            var tileNotification = GetTileNotification(options);

            return Task.Factory.StartNew(() => _tileUpdater.Update(tileNotification));
        }

        private static TileNotification GetTileNotification(PlaybackNotificationOptions options)
        {
            var wideTemplate = GetWideTemplate(options);
            var squareTempalte = GetSquateTempalte(options);
            wideTemplate.SquareContent = squareTempalte;

            var tileNotification = new TileNotification(wideTemplate.GetXml())
                {
                    ExpirationTime = new DateTimeOffset(DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)))
                };

            return tileNotification;
        }

        private static ITileWideSmallImageAndText03 GetWideTemplate(PlaybackNotificationOptions options)
        {
            var tile = TileContentFactory.CreateTileWideSmallImageAndText03();
            tile.Image.Src = options.ImageUrl;
            tile.TextBodyWrap.Text = string.Format("{0} - {1}", options.Title, options.Subtitle);
            tile.RequireSquareContent = true;

            return tile;
        }

        private static ITileSquarePeekImageAndText04 GetSquateTempalte(PlaybackNotificationOptions options)
        {
            var tile = TileContentFactory.CreateTileSquarePeekImageAndText04();
            tile.Image.Src = options.ImageUrl;
            tile.TextBodyWrap.Text = string.Format("{0} - {1}", options.Title, options.Subtitle);

            return tile;
        }
    }
}