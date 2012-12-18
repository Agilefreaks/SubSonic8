using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "album", Namespace = "http://subsonic.org/restapi")]
    public class Album : SubsonicModelBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        [XmlElement(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
        public List<Song> Songs { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Album; }
        }

        public Album()
        {
            Songs = new List<Song>();
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} tracks", SongCount));
        }
    }
}