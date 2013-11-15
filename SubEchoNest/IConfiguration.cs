namespace SubEchoNest
{
    public interface IConfiguration
    {
        string ApiKey { get; set; }

        string BaseUrl { get; set; }

        string RequestFormatWithApiKey { get; }
    }
}