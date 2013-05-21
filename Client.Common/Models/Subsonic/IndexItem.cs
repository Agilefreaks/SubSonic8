namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "index", Namespace = "http://subsonic.org/restapi")]
    [DataContract]
    public class IndexItem : MediaModelBase
    {
        #region Constructors and Destructors

        public IndexItem()
        {
            Artists = new List<Artist>();
        }

        #endregion

        #region Public Properties

        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        [DataMember]
        public List<Artist> Artists { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.Index;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} artists", Artists.Count));
        }

        #endregion
    }
}