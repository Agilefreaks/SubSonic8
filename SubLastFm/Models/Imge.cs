namespace SubLastFm.Models
{
    using System;
    using System.Xml.Serialization;

    public class Image
    {
        [XmlAttribute("size")]
        public ImageSizeEnum Size { get; set; }

        [XmlIgnore]
        public Uri Url { get; private set; }

        [XmlText]
        public string UrlString
        {
            get
            {
                return Url.ToString();
            }
            set
            {
                Url = new Uri(value);
            }
        }
    }
}