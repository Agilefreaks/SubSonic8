using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    public abstract class SubsonicModelBase : ISubsonicModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        public abstract SubsonicModelTypeEnum Type { get; }
    }
}