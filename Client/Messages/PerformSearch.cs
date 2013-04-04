namespace Subsonic8.Messages
{
    public class PerformSearch
    {
        public string QueryText { get; set; }

        public PerformSearch()
        {
        }

        public PerformSearch(string queryText)
        {
            QueryText = queryText;
        }
    }
}