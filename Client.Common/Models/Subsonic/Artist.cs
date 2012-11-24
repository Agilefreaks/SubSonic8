using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class Artist : SubsonicModelBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Artist; }
        }
    }
}