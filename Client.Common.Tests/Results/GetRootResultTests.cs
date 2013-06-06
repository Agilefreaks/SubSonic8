namespace Client.Common.Tests.Results
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
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
        public void CtorShouldSetConfiguration()
        {
            _subject.Configuration.Should().BeSameAs(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public async Task ExecuteShouldHandleHttpRequestException()
        {
            _subject.Response = () =>
                {
                    var tcr = new TaskCompletionSource<HttpStreamResult>();
                    tcr.SetResult(new HttpStreamResult { Exception = new HttpRequestException() });

                    return tcr.Task;
                };

            await Task.Run(() => _subject.Execute(new ActionExecutionContext()));

            _subject.Error.Should().BeOfType<HttpRequestException>();
        }

        [TestInitialize]
        public void Setup()
        {
            _subsonicServiceConfiguration = new SubsonicServiceConfiguration();
            _subject = new GetRootResult(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void ViewNameShoulBeCorrect()
        {
            _subject.ViewName.Should().Be("getMusicFolders.view");
        }

        #endregion
    }
}