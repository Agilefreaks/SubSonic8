namespace Client.Common.Results
{
    using System.Threading.Tasks;
    using Caliburn.Micro;

    public interface ITaskResult
    {
        #region Public Methods and Operators

        Task Execute(ActionExecutionContext context = null);

        #endregion
    }
}