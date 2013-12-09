namespace SubLastFm.Models
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "lfm")]
    public class LastFmResponse
    {       
        [XmlAttribute("status")]
        public LastFmResponseStatusEnum Status { get; set; }

        [XmlElement("error")]
        public Error Error { get; set; }
    }
}