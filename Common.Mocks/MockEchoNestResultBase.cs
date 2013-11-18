namespace Common.Mocks
{
    using SubEchoNest;
    using SubEchoNest.Results;

    public abstract class MockEchoNestResultBase<T> : MockRemoteXmlResultBase<T>, IEchoNestResultBase<T>
    {
        public IConfiguration Configuration { get; private set; }
    }
}