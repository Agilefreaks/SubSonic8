using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
    public class Song : SubsonicModelBase
    {
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

        public override SubsonicModelTypeEnum Type 
        {
            get { return IsVideo ? SubsonicModelTypeEnum.Video : SubsonicModelTypeEnum.Song; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Title, string.Format("Artist: {0}, Album: {1}", Artist, Album));
        }
    }
}