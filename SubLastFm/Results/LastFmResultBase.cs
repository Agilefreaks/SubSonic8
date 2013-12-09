namespace SubLastFm.Results
{
    using Common.Results;
    using IConfiguration = SubLastFm.IConfiguration;

    public abstract class LastFmResultBase<T> : RemoteXmlResultBase<T>, ILastFmResultBase<T>
    {
        public IConfiguration Configuration { get; private set; }

        public override string RequestUrl
        {
            get
            {
                return string.Format(
                    Configuration.RequestFormatWithApiKey, Configuration.BaseUrl, Configuration.ApiKey, ResourcePath);
            }
        }

        protected LastFmResultBase(IConfiguration configuration)
            : base(configuration, new LastFmApiCallErrorResponseHandler())
        {
            Configuration = configuration;
        }
    }
}