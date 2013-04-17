using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results 
{
    public class GetRootResult : ServiceResultBase<IList<MusicFolder>>, IGetRootResult
    {
        public override string ViewName { get { return "getMusicFolders.view"; } }

        public GetRootResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicFolder));

            Result = (from musicFolder in xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "musicFolders").Descendants(Namespace + "musicFolder")
                      select (MusicFolder)xmlSerializer.Deserialize(musicFolder.CreateReader())).ToList();
        }
    }
}