using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class GetSongResult : ServiceResultBase<Song>, IGetSongResult
    {
        public int Id { get; private set; }

        public override string ViewName
        {
            get { return "getSong.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetSongResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Song));
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "song");
            using (var xmlReader = xElement.CreateReader())
            {
                Result = (Song)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}