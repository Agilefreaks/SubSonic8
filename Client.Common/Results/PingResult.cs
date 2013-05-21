using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class PingResult : EmptyResponseResultBase, IPingResult
    {
        public Error ApiError { get; set; }

        public override string ViewName
        {
            get { return "ping.view"; }
        }

        public PingResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void HandleResponse(System.Xml.Linq.XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
            if (Result) return;
            var xmlSerializer = new XmlSerializer(typeof(Error));
            xElement = xElement.Element(Namespace + "error");
            using (var xmlReader = xElement.CreateReader())
            {
                ApiError = (Error)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}