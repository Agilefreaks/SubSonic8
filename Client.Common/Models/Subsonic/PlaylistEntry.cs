using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    public class PlaylistEntry : MediaModelBase
    {
        [XmlAttribute("title")]
        public override string Name { get; set; }

        [XmlAttribute("paArent")]
        public int ParentId { get; set; }

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        [XmlAttribute("bitRate")]
        public int BitRate { get; set; }

        [XmlAttribute("track")]
        public int Track { get; set; }

        [XmlAttribute("year")]
        public int Year { get; set; }

        [XmlAttribute("genre")]
        public int Genre { get; set; }

        [XmlAttribute("isVideo")]
        public int IsVideo { get; set; }

        [XmlAttribute("albumId")]
        public int AlbumId { get; set; }

        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Folder; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("Artist: {0}, Album: {1}", Artist, Album));
        }
    }
}