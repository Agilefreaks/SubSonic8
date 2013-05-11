using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    public abstract class MediaModelBase : SubsonicModelBase, IMediaModel
    {
        [XmlAttribute("coverArt")]
        [DataMember]
        public virtual string CoverArt { get; set; }
    }
}