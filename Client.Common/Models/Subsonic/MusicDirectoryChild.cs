namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
    public class MusicDirectoryChild : MediaModelBase
    {
        #region Public Properties

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        [XmlAttribute("isDir")]
        public bool IsDirectory { get; set; }

        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; }

        [XmlAttribute("name")]
        public override string Name
        {
            get
            {
                return Title;
            }

            set
            {
                Title = value;
            }
        }

        [XmlAttribute("parent")]
        public int Parent { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return IsDirectory
                           ? SubsonicModelTypeEnum.MusicDirectory
                           : IsVideo ? SubsonicModelTypeEnum.Video : SubsonicModelTypeEnum.Song;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return Type == SubsonicModelTypeEnum.MusicDirectory
                ? new Tuple<string, string>(Title, "")
                : new Tuple<string, string>(Title, Artist);
        }

        #endregion
    }
}