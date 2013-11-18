namespace SubEchoNest.Results
{
    using Common.Results;

    public abstract class EchoNestResultBase<T> : RemoteXmlResultBase<T>, IEchoNestResultBase<T>
    {
        public override string RequestUrl
        {
            get
            {
                return string.Format("{0}/{1}", Configuration.BaseUrl, ResourcePath);
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