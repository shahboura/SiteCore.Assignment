using System;
using System.Collections.Generic;
using System.Linq;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class SubscriberHandler : ISubscriberHandler
    {
        protected readonly IEventAggregator EventAggregator;
        public List<ISubscriber> Subscribers { get; } = new List<ISubscriber>();
        private int _counter = 1;

        public SubscriberHandler(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void DisplaySubscribers()
        {
            Subscribers.ForEach(s => Console.WriteLine(
                $"subscriber with name {s.Name} is {(!s.IsLive ? "not" : string.Empty)} live"));
        }

        public ISubscriber AddSubscriber<T>()
        {
            ISubscriber subscriber;

            if (typeof (T) == typeof (int))
            {
                subscriber = new SumSubscriber(EventAggregator);
            }
            else
            {
                subscriber = new PingSubscriber(EventAggregator);
            }

            subscriber.Name = $"{subscriber.GetType().Name} {_counter++}";
            subscriber.Subscribe();
            Subscribers.Add(subscriber);

            Console.WriteLine($"subscriber {subscriber.Name} created");
            return subscriber;
        }

        public void Subscribe(string name)
        {
            var subscriber = Subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                return;
            }

            subscriber.Subscribe();
            Console.WriteLine($"subscribed {name}");
        }

        public void Unsubscribe(string name)
        {
            var subscriber = Subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                return;
            }

            subscriber.Unsubscribe();
            Console.WriteLine($"unsubscribed {name}");
        }

        public void DeleteSubscriber(string name)
        {
            var matchedSubscribers = Subscribers.Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            matchedSubscribers.ForEach(s =>
            {
                // Manually unsubscribe to overcome instance action memory leak
                // One possible solution is MethodInfo
                s.Unsubscribe();
                Subscribers.Remove(s);
            });
            
            GC.Collect();
            Console.WriteLine($"subscriber {name} deleted");
        }
    }
}
