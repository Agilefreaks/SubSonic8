namespace Client.Common.Tests.Results
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using global::Common.Results;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetRootResultTests
    {
        #region Fields

        private GetRootResult _subject;

        private ISubsonicServiceConfiguration _subsonicServiceConfiguration;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ConstructorShouldSetConfiguration()
        {
            _subject.Configuration.Should().BeSameAs(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public async Task ExecuteShouldHandleHttpRequestException()
        {
            _subject.GetResourceFunc = () =>
                {
                    var taskCompletionSource = new TaskCompletionSource<HttpStreamResult>();
                    taskCompletionSource.SetResult(new HttpStreamResult { Exception = new HttpRequestException() });

                    return taskCompletionSource.Task;
                };

            await _subject.Execute();

            _subject.Error.Should().BeOfType<HttpRequestException>();
        }

        [TestInitialize]
        public void Setup()
        {
            _subsonicServiceConfiguration = new SubsonicServiceConfiguration();
            _subject = new GetRootResult(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void ViewNameShouldBeCorrect()
        {
            _subject.ResourcePath.Should().Be("getMusicFolders.view");
        }

        #endregion
    }
}