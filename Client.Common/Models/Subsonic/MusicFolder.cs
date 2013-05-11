using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "musicFolder", Namespace = "http://subsonic.org/restapi")]
    public class MusicFolder : MediaModelBase
    {
        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.Folder; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Empty);
        }
    }
}
