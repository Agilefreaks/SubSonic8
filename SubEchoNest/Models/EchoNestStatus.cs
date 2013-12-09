namespace SubEchoNest.Models
{
    using System.Xml.Serialization;
    using Common.Interfaces;

    [XmlRoot("status")]
    public class EchoNestStatus : IError
    {
        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlIgnore]
        public EchoNestStatusEnum RequestStatus
        {
            get
            {
                return (EchoNestStatusEnum)Code;
            }
        }
    }
}
