namespace SubLastFm
{
    public interface IConfiguration
    {
        string BaseUrl { get; }

        string ApiKey { get; }

        string RequestFormatWithApiKey { get; }
    }
}