using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Results;

namespace Client.Common
{
    public class SubsonicService : ResultBase
    {
        private const string ServiceUrl =
            "http://cristibadila.dynalias.com:33770/music/rest/{0}?u={1}&p={2}&v=1.8.0&c=SubSonic8";

        private const string UserName = "media";
        private const string Password = "media";
        private readonly HttpClient _client = new HttpClient();

        public IEnumerable<Models.Subsonic.Index> Result { get; set; }

        public override async void Execute(ActionExecutionContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Models.Subsonic.Index), new Type[] { typeof(Models.Subsonic.Artist) });
            string requestUrl = string.Format(ServiceUrl, "getIndexes.view", UserName, Password);

            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            XDocument xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            Result = (from index in xDocument.Element(ns + "subsonic-response").Element(ns + "indexes").Descendants(ns + "index") 
                      select (Models.Subsonic.Index)xmlSerializer.Deserialize(index.CreateReader())).ToList();

            OnCompleted();
        }
    }
}