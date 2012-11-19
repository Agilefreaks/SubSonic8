using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class ExpandedArtist
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("coverArt")]
        public string CovertArt { get; set; }

        [XmlAttribute("albumCount")]
        public int AlbumCount { get; set; }
    }
}