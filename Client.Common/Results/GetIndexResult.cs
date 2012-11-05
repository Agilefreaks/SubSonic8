using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public class GetIndexResult : ResultBase, IGetIndexResult
    {
        private readonly SubsonicServiceConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();

        public IEnumerable<Models.Subsonic.Index> Result { get; set; }

        public GetIndexResult(SubsonicServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(Models.Subsonic.Index), new[] { typeof(Models.Subsonic.Artist) });
            var requestUrl = string.Format(_configuration.ServiceUrl, "getIndexes.view", _configuration.Username, _configuration.Password);

            var response = await _client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            var xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            Result = (from index in xDocument.Element(ns + "subsonic-response").Element(ns + "indexes").Descendants(ns + "index")
                      select (Models.Subsonic.Index)xmlSerializer.Deserialize(index.CreateReader())).ToList();

            OnCompleted();

        }
    }
}