namespace SubLastFm.Models
{
    using System.Xml.Serialization;
    using Common.Interfaces;

    [XmlRoot(ElementName = "error")]
    public class Error : IError
    {
        #region Public Properties

        [XmlAttribute("code")]
        public int Code { get; set; }

        [XmlText]
        public string Message { get; set; }

        #endregion
    }
}