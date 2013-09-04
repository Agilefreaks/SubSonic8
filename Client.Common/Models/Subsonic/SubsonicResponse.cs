namespace Client.Common.Models.Subsonic
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "subsonic-response", Namespace = "http://subsonic.org/restapi")]
    public class SubsonicResponse
    {       
        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlElement("error")]
        public Error Error { get; set; }
    }
}