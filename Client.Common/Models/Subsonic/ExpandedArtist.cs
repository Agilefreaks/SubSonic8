namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class ExpandedArtist : Artist
    {
        #region Constructors and Destructors

        public ExpandedArtist()
        {
            Albums = new List<Album>();
        }

        #endregion

        #region Public Properties

        [XmlAttribute("albumCount")]
        public int AlbumCount { get; set; }

        [XmlElement(ElementName = "album", Namespace = "http://subsonic.org/restapi")]
        public List<Album> Albums { get; set; }

        [XmlIgnore]
        public override string CoverArt
        {
            get
            {
                return base.CoverArt;
            }

            set
            {
                base.CoverArt = value;
            }
        }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.Artist;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} albums", AlbumCount));
        }

        #endregion
    }
}