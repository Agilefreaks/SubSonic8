namespace Client.Common.Models.Subsonic
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "error", Namespace = "http://subsonic.org/restapi")]
    public class Error
    {
        #region Public Properties

        [XmlAttribute("code")]
        public int Code { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        #endregion
    }
}