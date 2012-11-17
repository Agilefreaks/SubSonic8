using System;
using Caliburn.Micro;
using Client.Common;
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

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new ShellViewModel(_eventAggregator, new MockSubsonicService());
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