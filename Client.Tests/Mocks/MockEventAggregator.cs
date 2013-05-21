namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Action = System.Action;

    public class MockEventAggregator : IEventAggregator
    {
        #region Constructors and Destructors

        public MockEventAggregator()
        {
            Messages = new List<object>();
        }

        #endregion

        #region Public Properties

        public List<object> Messages { get; set; }

        public Action<Action> PublicationThreadMarshaller { get; set; }

        public int PublishCallCount
        {
            get
            {
                return Messages.Count;
            }
        }

        public object Subscriber { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Publish(object message)
        {
            Messages.Add(message);
        }

        public void Publish(object message, Action<Action> marshal)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(object instance)
        {
            Subscriber = instance;
        }

        public void Unsubscribe(object instance)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}