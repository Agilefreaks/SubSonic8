namespace Client.Common.Results
{
    public interface IServiceResultBase : IResultBase
    {
        ISubsonicServiceConfiguration Configuration { get; }
    }
}