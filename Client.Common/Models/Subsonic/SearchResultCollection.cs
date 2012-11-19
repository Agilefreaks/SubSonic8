using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "searchResult3", Namespace = "http://subsonic.org/restapi")]
    public class SearchResultCollection
    {
        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        public List<ExpandedArtist> Artists { get; set; }

        [XmlElement(ElementName = "album", Namespace = "http://subsonic.org/restapi")]
        public List<Album> Albums { get; set; }

        [XmlElement(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
        public List<Song> Songs { get; set; }

        public string Query { get; set; }

        public SearchResultCollection()
        {
            Artists = new List<ExpandedArtist>();
            Albums = new List<Album>();
            Songs = new List<Song>();
        }
    }
}