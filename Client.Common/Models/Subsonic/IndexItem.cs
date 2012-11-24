using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "index", Namespace = "http://subsonic.org/restapi")]
    public class IndexItem :INavigableEntity
    {
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        public List<Artist> Artists { get; set; }

        public IndexItem()
        {
            Artists = new List<Artist>();
        }

        public NavigableTypeEnum Type
        {
            get { return NavigableTypeEnum.MusicDirectory; }
        }
    }
}