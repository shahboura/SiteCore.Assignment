using System;

namespace EventAggregator
{
    public class EventAggregator
    {
        public void Publish<T>(T data)
        {
            
        }

        public void Subscribe<T>(object context, Action<T> action)
        {
            
        }

        public void Unsubscribe(object context)
        {
            
        }

        public void Unsubscribe<T>()
        {
            
        }

        public void Unsubscribe<T>(object context)
        {
            
        }
    }
}
