using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockGetRootResult : IGetRootResult
    {
        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public string ViewName { get; private set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public string RequestUrl { get; private set; }

        public IList<IndexItem> Result { get; set; }

        public int ExecuteCallCount { get; private set; }

        public MockGetRootResult()
        {
            ExecuteCallCount = 0;
            Result = new List<IndexItem>();
        }

        public Task Execute(ActionExecutionContext context)
        {
            var tcr = new TaskCompletionSource<Stream>();
            tcr.SetResult(null);
            ExecuteCallCount++;

            return tcr.Task;
        }
    }
}