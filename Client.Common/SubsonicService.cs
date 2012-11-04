using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Results;

namespace Client.Common
{
    public class SubsonicService : ResultBase, ISubsonicService
    {
        private SubsonicServiceConfiguration _configuration;

        public SubsonicServiceConfiguration Configuration
        {
            get
            {
                return _configuration;
            }

            set 
            { 
                _configuration = value;
                NotifyOfPropertyChange();
            }
        }

        private readonly HttpClient _client = new HttpClient();

        public IEnumerable<Models.Subsonic.Index> Result { get; set; }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(Models.Subsonic.Index), new[] { typeof(Models.Subsonic.Artist) });
            var requestUrl = string.Format(Configuration.ServiceUrl, "getIndexes.view", Configuration.Username, Configuration.Password);

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