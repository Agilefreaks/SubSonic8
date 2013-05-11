using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class SearchResult : ServiceResultBase<SearchResultCollection>, ISearchResult
    {
        private readonly string _query;

        public override string RequestUrl
        {
            get
            {
                return string.Format("{0}&query={1}", base.RequestUrl, Uri.EscapeUriString(_query));
            }
        }

        public override string ViewName
        {
            get { return "search3.view "; }
        }

        public SearchResult(ISubsonicServiceConfiguration configuration, string query)
            : base(configuration)
        {
            _query = query;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(SearchResultCollection),
                                                  new[] { typeof(ExpandedArtist), typeof(Album), typeof(Song) });
            Result = (xDocument.Element(Namespace + "subsonic-response")
                               .Descendants(Namespace + "searchResult3")
                               .Select(
                                   searchResult =>
                                   {
                                       using (var xmlReader = searchResult.CreateReader())
                                       {
                                           return (SearchResultCollection)xmlSerializer.Deserialize(xmlReader);
                                       }
                                   })).Single();

            Result.Query = _query;
        }
    }
}