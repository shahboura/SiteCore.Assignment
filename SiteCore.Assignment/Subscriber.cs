using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class Subscriber<T> : ISubscriber
    {
        protected readonly IEventAggregator EventAggregator;
        protected Action<T> Action;

        public string Name { get; set; }
        public bool IsLive { get; set; }
        public T Data { get; set; }

        public Subscriber(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public virtual void Subscribe()
        {
            EventAggregator.Subscribe(this, Action);
            IsLive = true;
        }

        public virtual void Unsubscribe()
        {
            EventAggregator.Unsubscribe(this);
            IsLive = false;
        }
    }
}
