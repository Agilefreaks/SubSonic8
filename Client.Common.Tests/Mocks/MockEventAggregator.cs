using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Action = System.Action;

namespace Client.Common.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        public object Subscriber { get; private set; }

        public List<object> Messages { get; set; }

        public int PublishCallCount
        {
            get { return Messages.Count; }
        }

        public MockEventAggregator()
        {
            Messages = new List<object>();
        }

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
            Messages.Add(message);
        }

        public void Publish(object message, Action<Action> marshal)
        {
            throw new NotImplementedException();
        }

        public Action<Action> PublicationThreadMarshaller { get; set; }
    }
}
