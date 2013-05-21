namespace Client.Common.Results
{
    public interface ISuccessHandler<in T>
    {
        #region Public Methods and Operators

        void HandleSuccess(T result);

        #endregion
    }
}