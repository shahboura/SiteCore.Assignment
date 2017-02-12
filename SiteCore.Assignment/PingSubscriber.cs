using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class PingSubscriber : Subscriber<string>
    {
        public PingSubscriber(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Action = s =>
            {
                Console.WriteLine($"Pinging back {s} from {Name}");
            };
        }
    }
}
