﻿using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockGetAlbumResult : IGetAlbumResult
    {
        public int ExecuteCallCount { get; private set; }

        public Func<Common.Models.Subsonic.Album> GetResultFunc { get; set; }

        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public Common.Models.Subsonic.Album Result { get; private set; }

        public string ViewName { get; private set; }

        public string RequestUrl { get; private set; }

        public MockGetAlbumResult()
        {
            GetResultFunc = () => new Common.Models.Subsonic.Album();
        }

        public IServiceResultBase<Common.Models.Subsonic.Album> WithErrorHandler(IErrorHandler errorHandler)
        {
            return this;
        }

        public IServiceResultBase<Common.Models.Subsonic.Album> OnSuccess(Action<Common.Models.Subsonic.Album> onSuccess)
        {
            return this;
        }

        public Task Execute(ActionExecutionContext context = null)
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<Common.Models.Subsonic.Album>();
            Result = GetResultFunc();
            taskCompletionSource.SetResult(Result);

            return taskCompletionSource.Task;
        }
    }
}