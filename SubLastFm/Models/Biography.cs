namespace SubLastFm.Models
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    public class Biography
    {
        [XmlElement("published")]
        public string PublishDateString
        {
            get
            {
                return PublishDate.ToString();
            }
            set
            {
                PublishDate = DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        public DateTime PublishDate { get; set; }

        [XmlElement("summary")]
        public string Summary { get; set; }

        [XmlElement("content")]
        public string Content { get; set; }

        [XmlElement("placeformed")]
        public string PlaceFormed { get; set; }

        [XmlElement("yearformed")]
        public int? YearFormed { get; set; }
    }
}