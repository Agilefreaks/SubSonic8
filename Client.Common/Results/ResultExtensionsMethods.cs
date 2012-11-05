using System.Collections.Generic;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public static class ResultExtensionsMethods
    {
         public static void Execute(this IEnumerable<IResult> results, ActionExecutionContext context = null)
         {
             new SequentialResult(results.GetEnumerator()).Execute(context ?? new ActionExecutionContext());
         }
    }
}