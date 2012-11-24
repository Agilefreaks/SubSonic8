using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
    public class MusicDirectoryChild : INavigableEntity
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("parent")]
        public int Parent { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("coverArt")]
        public string CovertArt { get; set; }

        [XmlAttribute("isDir")]
        public bool IsDirectory { get; set; }

        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; }

        public NavigableTypeEnum Type
        {
            get { return NavigableTypeEnum.MusicDirectory; }
        }
    }
}