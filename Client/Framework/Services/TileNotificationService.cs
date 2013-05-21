namespace Subsonic8.Framework.Services
{
    using System;
    using System.Threading.Tasks;
    using NotificationsExtensions.TileContent;
    using Windows.UI.Notifications;

    public class TileNotificationService : ITileNotificationService
    {
        #region Fields

        private readonly TileUpdater _tileUpdater;

        #endregion

        #region Constructors and Destructors

        public TileNotificationService()
        {
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
        }

        #endregion

        #region Public Methods and Operators

        public Task Show(PlaybackNotificationOptions options)
        {
            var tileNotification = GetTileNotification(options);

            return Task.Factory.StartNew(() => _tileUpdater.Update(tileNotification));
        }

        #endregion

        #region Methods

        private static ITileSquarePeekImageAndText04 GetSquateTempalte(PlaybackNotificationOptions options)
        {
            var tile = TileContentFactory.CreateTileSquarePeekImageAndText04();
            tile.Image.Src = options.ImageUrl;
            tile.TextBodyWrap.Text = string.Format("{0} - {1}", options.Title, options.Subtitle);

            return tile;
        }

        private static TileNotification GetTileNotification(PlaybackNotificationOptions options)
        {
            var wideTemplate = GetWideTemplate(options);
            var squareTempalte = GetSquateTempalte(options);
            wideTemplate.SquareContent = squareTempalte;

            var tileNotification = new TileNotification(wideTemplate.GetXml())
                                       {
                                           ExpirationTime =
                                               new DateTimeOffset(
                                               DateTime.UtcNow.Add(
                                                   TimeSpan.FromMinutes(5)))
                                       };

            return tileNotification;
        }

        private static ITileWideImageAndText02 GetWideTemplate(PlaybackNotificationOptions options)
        {
            var tile = TileContentFactory.CreateTileWideImageAndText02();
            tile.Image.Src = options.ImageUrl;
            tile.TextCaption1.Text = options.Title;
            tile.TextCaption2.Text = options.Subtitle;
            tile.RequireSquareContent = true;

            return tile;
        }

        #endregion
    }
}