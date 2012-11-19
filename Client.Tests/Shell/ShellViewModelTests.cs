using System;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
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

        [TestInitialize]
        public void TestInitialize()
        {
            _mockSubsonicService = new MockSubsonicService();
            _subject = new ShellViewModel(_eventAggregator, _mockSubsonicService);
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
        public void PerformSubsonicSearch_Always_CallsSubsonicServiceSearchAndReturnsTheResult()
        {
            var callCount = 0;
            var searchResult = new SearchResult(new SubsonicServiceConfiguration(), "test");
            _mockSubsonicService.Search = s =>
                                              {
                                                  Assert.AreEqual("test", s);
                                                  callCount++;
                                                  return searchResult;
                                              };

            _subject.PerformSubsonicSearch("test");

            Assert.AreEqual(1, callCount);
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

        #endregion
    }
}