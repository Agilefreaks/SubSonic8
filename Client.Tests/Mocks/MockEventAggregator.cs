using System;
using Caliburn.Micro;
using Action = System.Action;

namespace Client.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        public object Subscriber { get; private set; }

        public int PublishCallCount { get; set; }

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
            PublishCallCount++;
        }

        public void Publish(object message, Action<Action> marshal)
        {
            throw new NotImplementedException();
        }

        public Action<Action> PublicationThreadMarshaller { get; set; }
    }
}
