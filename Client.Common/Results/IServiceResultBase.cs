namespace Client.Common.Results
{
    using Client.Common.Services.DataStructures.SubsonicService;
    using global::Common.Results;

    public interface IServiceResultBase<out T> : IRemoteXmlResultBase<T>
    {
        ISubsonicServiceConfiguration Configuration { get; }
    }
}