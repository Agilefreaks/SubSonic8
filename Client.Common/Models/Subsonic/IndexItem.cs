using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "index", Namespace = "http://subsonic.org/restapi")]
    [DataContract]
    public class IndexItem : SerializableModelBase<IndexItem>
    {
        private static readonly Type[] TypesOfChildren = new[] { typeof(Artist) };

        public new static IndexItem Deserialize(string data)
        {
            return Deserialize(data, TypesOfChildren);
        }

        [XmlAttribute("name")]
        [DataMember]
        public string Name { get; set; }

        [XmlElement(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
        [DataMember]
        public List<Artist> Artists { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Index; }
        }

        public IndexItem()
        {
            Artists = new List<Artist>();
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Format("{0} artists", Artists.Count));
        }
    }
}