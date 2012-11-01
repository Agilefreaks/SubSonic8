using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "index", Namespace = "http://subsonic.org/restapi")]
    public class Index
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        public List<Artist> Artists { get; set; }
    }
}