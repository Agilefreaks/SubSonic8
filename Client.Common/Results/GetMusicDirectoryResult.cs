using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public class GetMusicDirectoryResult : ResultBase, IGetMusicDirectoryResult
    {
        private readonly SubsonicServiceConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();
        private readonly int _id;

        public MusicDirectory Result { get; set; }

        public GetMusicDirectoryResult(SubsonicServiceConfiguration configuration, int id)
        {
            _configuration = configuration;
            _id = id;
        }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicDirectory), new[] { typeof(MusicDirectoryChild) });
            var requestUrl = string.Format(_configuration.ServiceUrl, "getMusicDirectory.view", _configuration.Username, _configuration.Password);
            requestUrl += string.Format("&id={0}", _id);

            var response = await _client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            var xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            var xElement = xDocument.Element(ns + "subsonic-response").Element(ns + "directory");
            Result = (MusicDirectory)xmlSerializer.Deserialize(xElement.CreateReader());

            OnCompleted();
        }
    }
}