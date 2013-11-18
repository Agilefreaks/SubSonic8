namespace SubEchoNest.Results
{
    using Common.Results;

    public interface IEchoNestResultBase<out T> : IRemoteXmlResultBase<T>
    {
        IConfiguration Configuration { get; }
    }
}