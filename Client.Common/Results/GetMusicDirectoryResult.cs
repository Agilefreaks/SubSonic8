using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public class GetMusicDirectoryResult : ServiceResultBase, IGetMusicDirectoryResult
    {
        private readonly int _id;

        public MusicDirectory Result { get; set; }

        public override string ViewName
        {
            get { throw new System.NotImplementedException(); }
        }

        public GetMusicDirectoryResult(SubsonicServiceConfiguration configuration, int id) : base(configuration)
        {
            _id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            throw new System.NotImplementedException();
        }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicDirectory), new[] { typeof(MusicDirectoryChild) });
            var requestUrl = string.Format(Configuration.ServiceUrl, "getMusicDirectory.view", Configuration.Username, Configuration.Password);
            requestUrl += string.Format("&id={0}", _id);

            var response = await Client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            var xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            var xElement = xDocument.Element(ns + "subsonic-response").Element(ns + "directory");
            Result = (MusicDirectory)xmlSerializer.Deserialize(xElement.CreateReader());

            OnCompleted();
        }
    }
}