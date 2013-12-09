namespace Client.Common.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetRootResult : ServiceResultBase<IList<MusicFolder>>, IGetRootResult
    {
        #region Constructors and Destructors

        public GetRootResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Public Properties

        public override string ResourcePath
        {
            get
            {
                return "getMusicFolders.view";
            }
        }

        #endregion

        #region Methods

        public override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicFolder));
            Result =
                xDocument.Element(Namespace + "subsonic-response")
                         .Element(Namespace + "musicFolders")
                         .Descendants(Namespace + "musicFolder")
                         .Select(
                             musicFolder =>
                                 {
                                     using (var xmlReader = musicFolder.CreateReader())
                                     {
                                         return (MusicFolder)xmlSerializer.Deserialize(xmlReader);
                                     }
                                 }).ToList();
        }

        #endregion
    }
}