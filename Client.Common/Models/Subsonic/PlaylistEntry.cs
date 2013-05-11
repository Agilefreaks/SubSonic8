using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "entry", Namespace = "http://subsonic.org/restapi")]
    public class PlaylistEntry : MediaModelBase, ISongModel
    {
        [XmlAttribute("parent")]
        public int ParentId { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        public override string Name
        {
            get { return Title; }
            set { Title = value; }
        }

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
        public string Genre { get; set; }

        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; }

        [XmlAttribute("albumId")]
        public int AlbumId { get; set; }

        [XmlAttribute("artistId")]
        public int ArtistId { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Folder; }
        }

        public static XmlAttributeOverrides GetXmlAttributeOverrides()
        {
            var xmlAttributeOverrides = new XmlAttributeOverrides();
            var xmlAttributes = new XmlAttributes { XmlAttribute = new XmlAttributeAttribute("title") };
            xmlAttributeOverrides.Add(typeof(SubsonicModelBase), "Name", xmlAttributes);

            return xmlAttributeOverrides;
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("Artist: {0}, Album: {1}", Artist, Album));
        }
    }
}