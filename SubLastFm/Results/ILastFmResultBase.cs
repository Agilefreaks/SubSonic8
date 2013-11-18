namespace SubLastFm.Results
{
    using Common.Results;
    using IConfiguration = SubLastFm.IConfiguration;

    public interface ILastFmResultBase<T> : IRemoteXmlResultBase<T>
    {
        IConfiguration Configuration { get; }
    }
}