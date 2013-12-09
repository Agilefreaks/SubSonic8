namespace SubEchoNest
{
    public class Configuration : IConfiguration
    {
        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public string RequestFormatWithApiKey
        {
            get
            {
                return "{0}/{2}?api_key={1}&format=xml";
            }
        }
    }
}