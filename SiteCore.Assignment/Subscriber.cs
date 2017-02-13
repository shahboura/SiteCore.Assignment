using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class Subscriber<T> : ISubscriber
    {
        protected readonly IEventAggregator EventAggregator;

        public string Name { get; set; }
        public bool IsLive { get; set; }
        public T Data { get; set; }

        public Subscriber(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public virtual void Subscribe()
        {
            EventAggregator.Subscribe<T>(this, Action);
            IsLive = true;
        }

        public virtual void Action(T data)
        {
        }

        public virtual void Unsubscribe()
        {
            EventAggregator.Unsubscribe(this);
            IsLive = false;
        }
    }
}
