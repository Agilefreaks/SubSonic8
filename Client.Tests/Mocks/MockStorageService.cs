namespace Client.Tests.Mocks
{
    using System;
    using System.Threading.Tasks;
    using Client.Common.Services;

    public class MockStorageService : IStorageService
    {
        #region Constructors and Destructors

        public MockStorageService()
        {
            LoadFunc = Activator.CreateInstance;
        }

        #endregion

        #region Public Properties

        public int LoadCallCount { get; private set; }

        public Func<Type, object> LoadFunc { get; set; }

        public int SaveCallCount { get; private set; }

        #endregion

        #region Properties

        protected int DeleteCallCount { get; private set; }

        #endregion

        #region Public Methods and Operators

        public Task Delete<T>()
        {
            DeleteCallCount++;

            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);

            return taskCompletionSource.Task;
        }

        public Task<string> GetData<T>()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            taskCompletionSource.SetResult(string.Empty);

            return taskCompletionSource.Task;
        }

        public Task<T> Load<T>()
        {
            LoadCallCount++;

            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult((T)LoadFunc(typeof(T)));

            return taskCompletionSource.Task;
        }

        public Task<T> Load<T>(string handle)
        {
            LoadCallCount++;

            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult((T)LoadFunc(typeof(T)));

            return taskCompletionSource.Task;
        }

        public Task Save<T>(T data)
        {
            SaveCallCount++;

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

        #endregion
    }
}