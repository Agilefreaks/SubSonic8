namespace Client.Common.Results
{
    using System.Net.Http;
    using System.Xml.Linq;
    using Client.Common.Services.DataStructures.SubsonicService;
    using global::Common.Results;

    public abstract class ServiceResultBase<T> : RemoteXmlResultBase<T>, IServiceResultBase<T>
    {
        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        public override string RequestUrl
        {
            get
            {

                return string.Format(
                    Configuration.RequestFormatWithUsernameAndPassword(),
                    ResourcePath,
                    Configuration.Username,
                    Configuration.EncodedPassword);
            }
        }

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        protected ServiceResultBase(ISubsonicServiceConfiguration configuration)
            : base(configuration, new SubsonicApiCallErrorResponseHandler())
        {
            Configuration = configuration;
        }

        protected override HttpRequestMessage CreateRequest()
        {
            var request = base.CreateRequest();
            request.Headers.Add("Authorization", string.Format("Basic {0}", Configuration.EncodedCredentials));

            return request;
        }
    }
}