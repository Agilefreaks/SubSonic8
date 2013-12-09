namespace SubEchoNest.Models
{
    using System.Xml.Serialization;

    [XmlRoot("biography")]
    public class Biography
    {
        [XmlElement("text")]
        public string Text { get; set; }

        [XmlElement("site")]
        public string Site { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }
    }
}
