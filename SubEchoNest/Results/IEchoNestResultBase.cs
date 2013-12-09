namespace SubEchoNest.Results
{
    using Common.Results;
    using IConfiguration = SubEchoNest.IConfiguration;

    public interface IEchoNestResultBase<out T> : IRemoteXmlResultBase<T>
    {
        IConfiguration Configuration { get; }
    }
}