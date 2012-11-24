using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class ExpandedArtist : INavigableEntity
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("coverArt")]
        public string CovertArt { get; set; }

        [XmlAttribute("albumCount")]
        public int AlbumCount { get; set; }

        public NavigableTypeEnum Type
        {
            get { return NavigableTypeEnum.Artist; }
        }
    }
}