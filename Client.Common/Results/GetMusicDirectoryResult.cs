using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services;

namespace Client.Common.Results
{
    public class GetMusicDirectoryResult : ServiceResultBase<MusicDirectory>, IGetMusicDirectoryResult
    {
        private readonly int _id;

        public int Id
        {
            get { return _id; }
        }

        public override string ViewName { get { return "getMusicDirectory.view"; } }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetMusicDirectoryResult(SubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            _id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicDirectory), new[] { typeof(MusicDirectoryChild) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "directory");
            Result = (MusicDirectory)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}