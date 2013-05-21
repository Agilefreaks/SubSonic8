namespace Client.Common.Results
{
    using System.Collections.Generic;
    using Caliburn.Micro;

    public static class ResultExtensionsMethods
    {
        #region Public Methods and Operators

        public static void Execute(this IEnumerable<IResult> results, ActionExecutionContext context = null)
        {
            new SequentialResult(results.GetEnumerator()).Execute(context ?? new ActionExecutionContext());
        }

        #endregion
    }
}