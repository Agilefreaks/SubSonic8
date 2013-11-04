namespace SubLastFm.Models
{
    using System.Xml.Serialization;

    public enum LastFmResponseStatusEnum
    {
        [XmlEnum("ok")]
        Ok,
        [XmlEnum("failed")]
        Failed
    }
}