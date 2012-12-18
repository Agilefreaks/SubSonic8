using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Subsonic8.Framework
{
    public class ToastsNotificationManager : INotificationManager
    {
        public void Show(NotificationOptions options)
        {
            var toastXml = BuildToast(options);
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private XmlDocument BuildToast(NotificationOptions options)
        {
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            
            // Build Image
            var images = template.GetElementsByTagName("image");
            ((XmlElement)images[0]).SetAttribute("src", options.ImageUrl);
            ((XmlElement)images[0]).SetAttribute("alt", "Cover Art");

            // Title and Subtitle
            var texts = template.GetElementsByTagName("text");
            texts[0].AppendChild(template.CreateTextNode(options.Title));
            texts[1].AppendChild(template.CreateTextNode(options.Subtitle));
            
            return template;
        }
    }
}
