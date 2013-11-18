namespace SubLastFm
{
    public interface IConfiguration : Common.Results.IConfiguration
    {
        string ApiKey { get; }

        string RequestFormatWithApiKey { get; }
    }
}