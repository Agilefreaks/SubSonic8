using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "directory", Namespace = "http://subsonic.org/restapi")]    
    public class MusicDirectory : SubsonicModelBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
        public List<MusicDirectoryChild> Children { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.MusicDirectory; }
        }
    }
}