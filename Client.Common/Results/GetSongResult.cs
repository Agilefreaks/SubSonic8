using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services;

namespace Client.Common.Results
{
    public class GetSongResult : ServiceResultBase<Song>, IGetSongResult
    {
        private readonly int _id;

        public int Id
        {
            get { return _id; }
        }

        public GetSongResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            _id = id;
        }

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

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Song), new[] { typeof(Song) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "song");
            Result = (Song)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}