namespace Client.Common.Results
{
    public interface IExtendedResult : IResultBase
    {
        IExtendedResult WithErrorHandler(IErrorHandler errorHandler);

        IExtendedResult OnSuccess(System.Action onSuccess);
    }
}