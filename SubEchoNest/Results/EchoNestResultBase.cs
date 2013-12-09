namespace SubEchoNest.Results
{
    using Common.Results;
    using IConfiguration = SubEchoNest.IConfiguration;

    public abstract class EchoNestResultBase<T> : RemoteXmlResultBase<T>, IEchoNestResultBase<T>
    {
        public override string RequestUrl
        {
            get
            {
                return string.Format(Configuration.RequestFormatWithApiKey, Configuration.BaseUrl, Configuration.ApiKey,
                    ResourcePath);
            }
        }

        public IConfiguration Configuration { get; private set; }

        protected EchoNestResultBase(IConfiguration configuration)
            : base(configuration, new EchoNestApiCallErrorResponseHandler())
        {
            Configuration = configuration;
        }
    }
}