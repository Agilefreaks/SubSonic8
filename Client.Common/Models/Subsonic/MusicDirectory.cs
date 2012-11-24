using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "directory", Namespace = "http://subsonic.org/restapi")]    
    public class MusicDirectory : INavigableEntity
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
        public List<MusicDirectoryChild> Children { get; set; }

        public NavigableTypeEnum Type
        {
            get { return NavigableTypeEnum.MusicDirectory; }
        }
    }
}