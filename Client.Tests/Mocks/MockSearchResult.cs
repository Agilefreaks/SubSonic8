namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;

    public class MockSearchResult : MockServiceResultBase<SearchResultCollection>, ISearchResult
    {
    }
}