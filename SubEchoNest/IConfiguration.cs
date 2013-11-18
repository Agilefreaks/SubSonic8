namespace SubEchoNest
{
    public interface IConfiguration : Common.Results.IConfiguration
    {
        string ApiKey { get; set; }

        string RequestFormatWithApiKey { get; }
    }
}