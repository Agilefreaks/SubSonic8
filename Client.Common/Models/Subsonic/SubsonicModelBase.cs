namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public abstract class SubsonicModelBase : ISubsonicModel
    {
        #region Public Properties

        [XmlAttribute("id")]
        [DataMember]
        public int Id { get; set; }

        [XmlAttribute("name")]
        [DataMember]
        public virtual string Name { get; set; }

        public abstract SubsonicModelTypeEnum Type { get; }

        #endregion

        #region Public Methods and Operators

        public virtual Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(GetType().Name, Id.ToString());
        }

        #endregion
    }
}