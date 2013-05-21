namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "musicFolder", Namespace = "http://subsonic.org/restapi")]
    public class MusicFolder : MediaModelBase
    {
        #region Public Properties

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return SubsonicModelTypeEnum.Folder;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(Name, string.Empty);
        }

        #endregion
    }
}