using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class ExpandedArtist : Artist
    {
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

        public ExpandedArtist()
        {
            Albums = new List<Album>();
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} albums", AlbumCount));
        }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.Artist;
            }
        }
    }
}