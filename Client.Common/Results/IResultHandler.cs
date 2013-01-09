namespace Client.Common.Results
{
    public interface IResultHandler<in T> : IErrorHandler, ISuccessHandler<T>
    {
    }
}