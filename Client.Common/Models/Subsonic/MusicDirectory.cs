using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "directory", Namespace = "http://subsonic.org/restapi")]    
    public class MusicDirectory : SubsonicModelBase
    {
        [XmlElement(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
        public List<MusicDirectoryChild> Children { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get { return SubsonicModelTypeEnum.MusicDirectory; }
        }

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, base.GetDescription().Item2);
        }
    }
}