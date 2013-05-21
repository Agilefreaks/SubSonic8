namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "song", Namespace = "http://subsonic.org/restapi")]
    public class Song : MusicDirectoryChild, ISongModel
    {
        #region Public Properties

        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return IsVideo ? SubsonicModelTypeEnum.Video : SubsonicModelTypeEnum.Song;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Title, string.Format("Artist: {0}, Album: {1}", Artist, Album));
        }

        #endregion
    }
}