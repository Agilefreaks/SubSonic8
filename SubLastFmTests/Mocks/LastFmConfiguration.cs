namespace SubLastFmTests.Mocks
{
    using SubLastFm;

    public class MockLastFmConfigurationProvider : IConfigurationProvider
    {
        public IConfiguration Configuration { get; private set; }

        public MockLastFmConfigurationProvider()
        {
            Configuration = new Configuration
            {
                ApiKey = "testKey",
                BaseUrl = "http://test.com",
            };
        }
    }
}
