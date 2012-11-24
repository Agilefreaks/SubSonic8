using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class ExpandedArtist : SubsonicModelBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("coverArt")]
        public string CovertArt { get; set; }

        [XmlAttribute("albumCount")]
        public int AlbumCount { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Artist; }
        }
    }
}