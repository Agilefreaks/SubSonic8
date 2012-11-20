using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Search;
using Subsonic8.Shell;
using Action = System.Action;

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
        public void HandleWithPlayFileShouldSetSource()
        {
            _subject.Handle(new PlayFile { Id = 42 });
            _subject.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
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
        public async Task PerformSubsonicSearchAlwaysCallsNavigatesToSearchResultCall()
        {
            var called = false;
            var searchResult = new MockSearchResult(new SubsonicServiceConfiguration(), "test");
            _mockSubsonicService.Search = s => searchResult;
            _subject.NavigateToSearhResult = collection => { called = true; };

            await _subject.PerformSubsonicSearch("test");

            Assert.IsTrue(called);
        }

        #region Mocks

        internal class MockSubsonicService : SubsonicService
        {
            public override Uri GetUriForFileWithId(int id)
            {
                return new Uri(string.Format("http://subsonic.org?id={0}", id));
            }
        }

        internal class MockEventAggregator : IEventAggregator
        {
            public object Subscriber { get; private set; }

            public void Subscribe(object instance)
            {
                Subscriber = instance;
            }

            public void Unsubscribe(object instance)
            {
                throw new NotImplementedException();
            }

            public void Publish(object message)
            {
                throw new NotImplementedException();
            }

            public void Publish(object message, Action<Action> marshal)
            {
                throw new NotImplementedException();
            }

            public Action<Action> PublicationThreadMarshaller { get; set; }
        }

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