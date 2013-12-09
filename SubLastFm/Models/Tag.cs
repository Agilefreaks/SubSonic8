namespace SubLastFm.Models
{
    using System.Xml.Serialization;

    public class Tag
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}