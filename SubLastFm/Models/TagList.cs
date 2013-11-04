namespace SubLastFm.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class TagList
    {
        public TagList()
        {
            Items = new List<Tag>();
        }

        [XmlElement("tag")]
        public List<Tag> Items { get; set; }
    }
}