using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
    public class Song : MusicDirectoryChild
    {
        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

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