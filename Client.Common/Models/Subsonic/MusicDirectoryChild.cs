using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
    public class MusicDirectoryChild : MediaModelBase
    {
        [XmlAttribute("parent")]
        public int Parent { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("name")]
        public override string Name
        {
            get { return Title; }
            set { Title = value; }
        }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("isDir")]
        public bool IsDirectory { get; set; }

        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; }

        [XmlAttribute("duration")]
        public int Duration { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return IsDirectory
                           ? SubsonicModelTypeEnum.MusicDirectory
                           : IsVideo ? SubsonicModelTypeEnum.Video : SubsonicModelTypeEnum.Song;
            }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Title, Artist);
        }
    }
}