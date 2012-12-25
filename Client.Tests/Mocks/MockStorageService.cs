using System;
using System.Threading.Tasks;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockStorageService : IStorageService
    {
        public int LoadCallCount { get; set; }

        public int SaveCallCount { get; set; }

        public Func<Type, object> LoadFunc { get; set; }

        public MockStorageService()
        {
            LoadFunc = Activator.CreateInstance;
        }

        public Task Save<T>(T data)
            where T : class
        {
            SaveCallCount++;

            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);

            return taskCompletionSource.Task;
        }

        public Task<T> Load<T>()
            where T : class
        {
            LoadCallCount++;

            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult(LoadFunc(typeof(T)) as T);

            return taskCompletionSource.Task;
        }
    }
}