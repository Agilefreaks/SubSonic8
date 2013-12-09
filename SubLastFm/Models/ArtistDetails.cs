namespace SubLastFm.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "artist")]
    public class ArtistDetails
    {
        public ArtistDetails()
        {
            Images = new List<Image>();
            Tags = new TagList();
            Biography = new Biography();
            Band = new Band();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("bandmembers")]
        public Band Band { get; set; }

        [XmlIgnore]
        public Uri Url { get; private set; }

        [XmlElement("url")]
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

        [XmlElement("image")]
        public List<Image> Images { get; set; }

        [XmlElement("tags")]
        public TagList Tags { get; set; }

        [XmlElement("bio")]
        public Biography Biography { get; set; }

        public Image LargestImage()
        {
            return Images.OrderByDescending(image => image.Size).FirstOrDefault();
        }
    }
}