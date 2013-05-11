using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "playlist", Namespace = "http://subsonic.org/restapi")]
    public class Playlist : SubsonicModelBase
    {
        [XmlAttribute("fan")]
        public string Fan { get; set; }

        [XmlAttribute("owner")]
        public string Owner { get; set; }

        [XmlAttribute("public")]
        public bool Public { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        [XmlAttribute("created")]
        public DateTime Created { get; set; }

        [XmlElement(ElementName = "entry", Namespace = "http://subsonic.org/restapi")]
        public List<PlaylistEntry> Entries { get; set; }

        public Playlist()
        {
            Entries = new List<PlaylistEntry>();
        }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Playlist; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} items", SongCount));
        }
    }
}