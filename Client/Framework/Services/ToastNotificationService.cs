namespace Subsonic8.Framework.Services
{
    using System.Threading.Tasks;
    using NotificationsExtensions.ToastContent;
    using Windows.UI.Notifications;

    public class ToastNotificationService : IToastNotificationService
    {
        #region Fields

        private readonly ToastNotifier _toastNotifier;

        #endregion

        #region Constructors and Destructors

        public ToastNotificationService()
        {
            _toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }

        #endregion

        #region Public Properties

        public bool EnableNotifications { get; set; }

        #endregion

        #region Public Methods and Operators

        public Task Show(PlaybackNotificationOptions options)
        {
            if (EnableNotifications)
            {
                _toastNotifier.Show(BuildToast(options));
            }
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        #region Methods

        private static ToastNotification BuildToast(PlaybackNotificationOptions options)
        {
            var toast = ToastContentFactory.CreateToastImageAndText02();
            toast.Image.Src = options.ImageUrl;
            toast.Image.Alt = "Cover Art";
            toast.TextHeading.Text = options.Title;
            toast.TextBodyWrap.Text = options.Subtitle;

            return new ToastNotification(toast.GetXml());
        }

        #endregion
    }
}