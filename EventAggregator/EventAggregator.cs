using System;
using System.Collections.Generic;

namespace EventAggregator
{
    public class EventAggregator
    {
        private readonly object _lockObject = new object();

        private readonly Dictionary<Type, List<Subscriber>> _subscribers =
            new Dictionary<Type, List<Subscriber>>();

        public void Publish<T>(T data)
        {
            var type = typeof (T);
            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var subscribers = _subscribers[type];
            lock (_lockObject)
            {
                subscribers.RemoveAll(s => !s.Context.IsAlive);
            }

            subscribers.ForEach(s => ((Action<T>) s.Action)(data));
        }

        public void Subscribe<T>(object context, Action<T> action)
        {
            var type = typeof (T);
            var subscriber = new Subscriber
            {
                Context = new WeakReference(context),
                Action = action
            };

            lock (_lockObject)
            {
                if (_subscribers.ContainsKey(type))
                {
                    _subscribers[type].Add(subscriber);
                }
                else
                {
                    _subscribers[type] = new List<Subscriber> {subscriber};
                }
            }
        }

        public void Unsubscribe(object context)
        {
            lock (_lockObject)
            {
                foreach (var subscribers in _subscribers.Values)
                {
                    subscribers.RemoveAll(
                        s => !s.Context.IsAlive || s.Context.Target == context);
                }
            }
        }

        public void Unsubscribe<T>()
        {
            _subscribers.Remove(typeof (T));
        }

        public void Unsubscribe<T>(object context)
        {
            List<Subscriber> subscribers;

            if (!_subscribers.TryGetValue(typeof (T), out subscribers))
            {
                return;
            }

            lock (_lockObject)
            {
                subscribers.RemoveAll(
                    s => !s.Context.IsAlive || s.Context.Target == context);
            }
        }
    }
}
