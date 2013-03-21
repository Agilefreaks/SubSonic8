using System;
using System.Threading.Tasks;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockStorageService : IStorageService
    {
        public int LoadCallCount { get; private set; }

        public int SaveCallCount { get; private set; }

        protected int DeleteCallCount { get; private set; }

        public Func<Type, object> LoadFunc { get; set; }

        public MockStorageService()
        {
            LoadFunc = Activator.CreateInstance;
        }

        public Task Save<T>(T data)
        {
            SaveCallCount++;

            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);

            return taskCompletionSource.Task;
        }

        public Task<T> Load<T>()
        {
            LoadCallCount++;

            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult((T)LoadFunc(typeof(T)));

            return taskCompletionSource.Task;
        }

        public Task Delete<T>()
        {
            DeleteCallCount++;

            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);

            return taskCompletionSource.Task;
        }

        public Task Save<T>(T data, string handle)
        {
            SaveCallCount++;

            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);

            return taskCompletionSource.Task;
        }

        public Task<T> Load<T>(string handle)
        {
            LoadCallCount++;

            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult((T)LoadFunc(typeof(T)));

            return taskCompletionSource.Task;
        }
    }
}