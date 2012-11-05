using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "child", Namespace = "http://subsonic.org/restapi")]        
    public class MusicDirectoryChild
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

        [XmlAttribute("isDir")]
        public bool IsDirectory { get; set; }
    }
}