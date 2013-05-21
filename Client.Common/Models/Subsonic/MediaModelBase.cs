namespace Client.Common.Models.Subsonic
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public abstract class MediaModelBase : SubsonicModelBase, IMediaModel
    {
        #region Public Properties

        [XmlAttribute("coverArt")]
        [DataMember]
        public virtual string CoverArt { get; set; }

        #endregion
    }
}