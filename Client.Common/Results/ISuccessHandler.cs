namespace Client.Common.Results
{
    public interface ISuccessHandler<in T>
    {
        void HandleSuccess(T result);
    }
}