namespace Common.Mocks
{
    using SubEchoNest;

    public class MockEchoNestConfigurationProvider : IConfigurationProvider
    {
        public IConfiguration Configuration { get; private set; }

        public MockEchoNestConfigurationProvider()
        {
            Configuration = new Configuration
            {
                ApiKey = "testKey",
                BaseUrl = "http://test.com",
            };
        }
    }
}
