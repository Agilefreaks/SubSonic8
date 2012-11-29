using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Shell;

namespace Client.Tests.Shell
{
    [TestClass]
    public class ShellViewModelTests
    {
        private IShellViewModel _subject;
        private readonly MockEventAggregator _eventAggregator = new MockEventAggregator();
        private MockSubsonicService _mockSubsonicService;
        private MockNavigationService _mockNavigationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockSubsonicService = new MockSubsonicService();
            _mockNavigationService = new MockNavigationService();
            _subject = new ShellViewModel(_eventAggregator, _mockSubsonicService, _mockNavigationService);
        }

        [TestMethod]
        public void CtorShouldSubscribeToEventAggregator()
        {
            _eventAggregator.Subscriber.Should().Be(_subject);
        }

        [TestMethod]
        public async void PerformSubsonicSearchAlwaysCallsSubsonicServiceSearchAndReturnsTheResult()
        {
            var callCount = 0;
            var searchResult = new MockSearchResult(new SubsonicServiceConfiguration(), "test");
            _subject.NavigateToSearhResult = (collection) => { };
            _mockSubsonicService.Search = s =>
                                              {
                                                  Assert.AreEqual("test", s);
                                                  callCount++;
                                                  return searchResult;
                                              };

            await _subject.PerformSubsonicSearch("test");

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PerformSubsonicSearchAlwaysCallsNavigatesToSearchResult()
        {
            var called = false;
            var searchResult = new MockSearchResult(new SubsonicServiceConfiguration(), "test");
            _mockSubsonicService.Search = s => searchResult;
            _subject.NavigateToSearhResult = collection => { called = true; };

            await _subject.PerformSubsonicSearch("test");

            Assert.IsTrue(called);
        }

        # region Mocks
        internal class MockSearchResult : SearchResult
        {
            public bool ExecuteCalled { get; set; }

            public MockSearchResult(ISubsonicServiceConfiguration configuration, string query) 
                : base(configuration, query)
            {
                ExecuteCalled = false;
            }

            public override async Task Execute(ActionExecutionContext context = null)
            {
                await Task.Run(() => ExecuteCalled = true);
            }
        }

        #endregion
    }
}