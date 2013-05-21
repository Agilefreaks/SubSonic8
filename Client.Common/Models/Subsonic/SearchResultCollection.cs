namespace Client.Common.Models.Subsonic
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "searchResult3", Namespace = "http://subsonic.org/restapi")]
    public class SearchResultCollection
    {
        #region Constructors and Destructors

        public SearchResultCollection()
        {
            Artists = new List<ExpandedArtist>();
            Albums = new List<Album>();
            Songs = new List<Song>();
        }

        #endregion

        #region Public Properties

        [XmlElement(ElementName = "album", Namespace = "http://subsonic.org/restapi")]
        public List<Album> Albums { get; set; }

        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        public List<ExpandedArtist> Artists { get; set; }

        public string Query { get; set; }

        [XmlElement(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
        public List<Song> Songs { get; set; }

        #endregion
    }
}