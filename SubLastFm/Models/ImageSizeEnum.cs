namespace SubLastFm.Models
{
    using System.Xml.Serialization;

    public enum ImageSizeEnum
    {
        [XmlEnum("small")]
        Small,
        [XmlEnum("medium")]
        Medium,
        [XmlEnum("large")]
        Large,
        [XmlEnum("extralarge")]
        ExtraLarge,
        [XmlEnum("mega")]
        Mega
    }
}