using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public class GetRootResult : ServiceResultBase, IGetRootResult
    {
        public IList<IndexItem> Result { get; set; }

        public GetRootResult(ISubsonicServiceConfiguration configuration) : base(configuration)
        {
        }

        public override async void Execute(ActionExecutionContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(IndexItem), new[] { typeof(Artist) });
            var requestUrl = string.Format(Configuration.ServiceUrl, "getIndexes.view", Configuration.Username, Configuration.Password);

            var response = await Client.GetAsync(requestUrl);
            var stream = await response.Content.ReadAsStreamAsync();

            var xDocument = XDocument.Load(stream);

            XNamespace ns = "http://subsonic.org/restapi";
            Result = (from musicFolder in xDocument.Element(ns + "subsonic-response").Element(ns + "indexes").Descendants(ns + "index")
                      select (IndexItem)xmlSerializer.Deserialize(musicFolder.CreateReader())).ToList();

            OnCompleted();
        }
    }
}