namespace Client.Common.Models.Subsonic
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "artist", Namespace = "http://subsonic.org/restapi")]
    public class Artist : MediaModelBase
    {
        #region Public Properties

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
            return new Tuple<string, string>(Name, Name);
        }

        #endregion
    }
}