namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "directory", Namespace = "http://subsonic.org/restapi")]
    public class MusicDirectory : MediaModelBase
    {
        #region Public Properties

        [XmlElement(ElementName = "child", Namespace = "http://subsonic.org/restapi")]
        public List<MusicDirectoryChild> Children { get; set; }

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.MusicDirectory;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, "");
        }

        #endregion
    }
}