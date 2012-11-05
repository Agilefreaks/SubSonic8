using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public class GetRootResult : ResultBase, IGetRootResult
    {
        private readonly SubsonicServiceConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();

        public IList<IndexItem> Result { get; set; }

        public GetRootResult(SubsonicServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(IndexItem), new[] { typeof(Artist) });
            var requestUrl = string.Format(_configuration.ServiceUrl, "getIndexes.view", _configuration.Username, _configuration.Password);

            var response = await _client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            var xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            Result = (from musicFolder in xDocument.Element(ns + "subsonic-response").Element(ns + "indexes").Descendants(ns + "index")
                      select (IndexItem)xmlSerializer.Deserialize(musicFolder.CreateReader())).ToList();

            OnCompleted();

        }
    }
}