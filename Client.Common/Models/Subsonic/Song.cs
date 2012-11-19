using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
    public class Song
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("parent")]
        public string Parent { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        [XmlAttribute("coverArt")]
        public string CoverArt { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; }
    }
}