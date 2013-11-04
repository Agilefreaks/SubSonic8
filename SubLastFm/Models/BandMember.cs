namespace SubLastFm.Models
{
    using System.Xml.Serialization;

    public class BandMember
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}