namespace Client.Common.Models.Subsonic
{
    using System.Xml.Serialization;
    using global::Common.Interfaces;

    [XmlRoot(ElementName = "error", Namespace = "http://subsonic.org/restapi")]
    public class Error : IError
    {
        #region Public Properties

        [XmlAttribute("code")]
        public int Code { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        #endregion
    }
}