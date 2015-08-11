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

        public bool UseSound { get; set; }

        #endregion

        #region Public Methods and Operators

        public Task Show(PlaybackNotificationOptions options)
        {
            var toast = BuildToast(options);
            var toastNotification = new ToastNotification(toast.GetXml());

            return Task.Factory.StartNew(() => _toastNotifier.Show(toastNotification));
        }

        #endregion

        #region Methods

        private IToastImageAndText02 BuildToast(PlaybackNotificationOptions options)
        {
            var toast = ToastContentFactory.CreateToastImageAndText02();
            toast.Image.Src = options.ImageUrl;
            toast.Image.Alt = "Cover Art";
            toast.TextHeading.Text = options.Title;
            toast.TextBodyWrap.Text = options.Subtitle;

            return toast;
        }

        #endregion
    }
}