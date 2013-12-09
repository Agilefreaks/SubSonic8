namespace Common.Mocks
{
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;

    public abstract class MockServiceResultBase<T> : MockRemoteXmlResultBase<T>, IServiceResultBase<T>
    {
        public ISubsonicServiceConfiguration Configuration { get; private set; }
    }
}