namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "playlist", Namespace = "http://subsonic.org/restapi")]
    public class Playlist : SubsonicModelBase
    {
        #region Constructors and Destructors

        public Playlist()
        {
            Entries = new List<PlaylistEntry>();
        }

        #endregion

        #region Public Properties

        [XmlAttribute("created")]
        public DateTime Created { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        [XmlElement(ElementName = "entry", Namespace = "http://subsonic.org/restapi")]
        public List<PlaylistEntry> Entries { get; set; }

        [XmlAttribute("fan")]
        public string Fan { get; set; }

        [XmlAttribute("owner")]
        public string Owner { get; set; }

        [XmlAttribute("public")]
        public bool Public { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.Playlist;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} items", SongCount));
        }

        #endregion
    }
}