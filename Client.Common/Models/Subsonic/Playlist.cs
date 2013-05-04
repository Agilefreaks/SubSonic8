using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "Playlist", Namespace = "http://subsonic.org/restapi")]
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
        public List<MusicDirectoryChild> Entries { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Playlist; }
        }
    }
}