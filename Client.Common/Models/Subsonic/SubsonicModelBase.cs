using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [DataContract]
    public abstract class SubsonicModelBase : ISubsonicModel
    {
        [XmlAttribute("id")]
        [DataMember]
        public int Id { get; set; }

        public abstract SubsonicModelTypeEnum Type { get; }

        [XmlAttribute("name")]
        [DataMember]
        public string Name { get; set; }

        [XmlAttribute("coverArt")]
        [DataMember]
        public virtual string CoverArt { get; set; }

        public virtual Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(GetType().Name, Id.ToString());
        }
    }
}