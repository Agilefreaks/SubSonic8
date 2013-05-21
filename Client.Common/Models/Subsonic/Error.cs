using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "error", Namespace = "http://subsonic.org/restapi")]
    public class Error
    {
        [XmlAttribute("code")]
        public int Code { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }
    }
}