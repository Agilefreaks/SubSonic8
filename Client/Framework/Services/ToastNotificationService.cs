using System.Threading.Tasks;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.ToastContent;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Subsonic8.Framework.Services
{
    public class ToastNotificationService : IToastNotificationService
    {
        private readonly ToastNotifier _toastNotifier;

        public bool UseSound { get; set; }

        public ToastNotificationService()
        {
            _toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }

        public Task Show(PlaybackNotificationOptions options)
        {
            var toast = BuildToast(options);
            var toastNotification = new ToastNotification(toast.GetXml());

            return Task.Factory.StartNew(() => _toastNotifier.Show(toastNotification));
        }

        private IToastImageAndText02 BuildToast(PlaybackNotificationOptions options)
        {
            var toast = ToastContentFactory.CreateToastImageAndText02();
            toast.Image.Src = options.ImageUrl;
            toast.Image.Alt = "Cover Art";
            toast.TextHeading.Text = options.Title;
            toast.TextBodyWrap.Text = options.Subtitle;
            toast.Audio.Content = UseSound ? ToastAudioContent.Default : ToastAudioContent.Silent;

            return toast;
        }
    }
}
