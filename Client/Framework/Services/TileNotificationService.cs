using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
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
            var squareTempalte = GetSquateTempalte(options);
            var wideTemplate = GetWideTemplate(options);
            CombineTemplates(squareTempalte, wideTemplate);
            var tileNotification = new TileNotification(squareTempalte)
                {
                    ExpirationTime = new DateTimeOffset(DateTime.UtcNow, TimeSpan.FromMinutes(5))
                };

            return tileNotification;
        }

        private static XmlDocument GetWideTemplate(PlaybackNotificationOptions options)
        {
            var template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideSmallImageAndText03);

            var imageElement = template.GetElementsByTagName("image").First();
            Debug.Assert(imageElement != null, "imageElement != null");
            imageElement.Attributes.First(attr => attr.NodeName == "src").NodeValue = options.ImageUrl;

            var textElement = GetTextElementById(template, "1");
            textElement.InnerText = string.Format("{0} - {1}", options.Title, options.Subtitle);

            return template;
        }

        private static XmlDocument GetSquateTempalte(PlaybackNotificationOptions options)
        {
            var template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText04);

            var imageElement = template.GetElementsByTagName("image").First();
            Debug.Assert(imageElement != null, "imageElement != null");
            imageElement.Attributes.First(attr => attr.NodeName == "src").NodeValue = options.ImageUrl;

            var textElement = GetTextElementById(template, "1");
            textElement.InnerText = string.Format("{0} - {1}", options.Title, options.Subtitle);

            return template;
        }

        private static IXmlNode GetTextElementById(XmlDocument xmlDocument, string id)
        {
            return xmlDocument.GetElementsByTagName("text").First(element => GetId(element) == id);
        }

        private static string GetId(IXmlNode element)
        {
            return GetIdAttribute(element).NodeValue.ToString();
        }

        private static IXmlNode GetIdAttribute(IXmlNode element)
        {
            return element.Attributes.First(attr => attr.NodeName == "id");
        }

        private static void CombineTemplates(XmlDocument firstTemplate, XmlDocument secondTemplate)
        {
            var nodeCopy = firstTemplate.ImportNode(secondTemplate.GetElementsByTagName("binding").First(), true);
            firstTemplate.GetElementsByTagName("visual").First().AppendChild(nodeCopy);
        }
    }
}