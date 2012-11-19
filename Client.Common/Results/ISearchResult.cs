using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface ISearchResult : IResultBase
    {
        SearchResultCollection Result { get; set; }
    }
}