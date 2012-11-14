using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public class GetRootResult : ServiceResultBase, IGetRootResult
    {
        public IList<IndexItem> Result { get; set; }

        public override string ViewName { get { return "getIndexes.view"; }}

        public GetRootResult(ISubsonicServiceConfiguration configuration) : base(configuration)
        {
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(IndexItem), new[] { typeof(Artist) });

            Result = (from musicFolder in xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "indexes").Descendants(Namespace + "index")
                      select (IndexItem)xmlSerializer.Deserialize(musicFolder.CreateReader())).ToList();
        }
    }
}