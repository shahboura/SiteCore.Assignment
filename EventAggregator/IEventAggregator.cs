using System;

namespace EventAggregator
{
    public interface IEventAggregator
    {
        void Publish<T>(T data);
        void Subscribe<T>(object context, Action<T> action);
        void Unsubscribe(object context);
        void Unsubscribe<T>();
        void Unsubscribe<T>(object context);
    }
}