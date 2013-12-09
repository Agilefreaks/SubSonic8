namespace SubLastFm.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Band
    {
        public Band()
        {
            Members = new List<BandMember>();
        }

        [XmlElement("member")]
        public List<BandMember> Members { get; set; }
    }
}