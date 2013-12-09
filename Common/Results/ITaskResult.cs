namespace Common.Results
{
    using System.Threading.Tasks;

    public interface ITaskResult
    {
        #region Public Methods and Operators

        Task Execute();

        #endregion
    }
}