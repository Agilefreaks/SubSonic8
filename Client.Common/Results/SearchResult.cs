namespace Client.Common.Results
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class SearchResult : ServiceResultBase<SearchResultCollection>, ISearchResult
    {
        #region Fields

        private readonly string _query;

        #endregion

        #region Constructors and Destructors

        public SearchResult(ISubsonicServiceConfiguration configuration, string query)
            : base(configuration)
        {
            _query = query;
        }

        #endregion

        #region Public Properties

        public override string RequestUrl
        {
            get
            {
                return string.Format("{0}&query={1}", base.RequestUrl, Uri.EscapeUriString(_query));
            }
        }

        public override string ViewName
        {
            get
            {
                return "search3.view ";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(
                typeof(SearchResultCollection), new[] { typeof(ExpandedArtist), typeof(Album), typeof(Song) });
            Result =
                xDocument.Element(Namespace + "subsonic-response")
                         .Descendants(Namespace + "searchResult3")
                         .Select(
                             searchResult =>
                                 {
                                     using (var xmlReader = searchResult.CreateReader())
                                     {
                                         return (SearchResultCollection)xmlSerializer.Deserialize(xmlReader);
                                     }
                                 }).Single();

            Result.Query = _query;
        }

        #endregion
    }
}