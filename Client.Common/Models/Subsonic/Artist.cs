using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class Artist : MediaModelBase
    {
        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.MusicDirectory; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, Name);
        }
    }
}