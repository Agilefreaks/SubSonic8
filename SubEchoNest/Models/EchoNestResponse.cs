namespace SubEchoNest.Models
{
    using System.Xml.Serialization;

    [XmlRoot("response")]
    public class EchoNestResponse
    {
        [XmlElement("status")]
        public EchoNestStatus Status { get; set; }
    }
}
