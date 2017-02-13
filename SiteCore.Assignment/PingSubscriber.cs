using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class PingSubscriber : Subscriber<string>
    {
        public PingSubscriber(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public override void Action(string data)
        {
            Console.WriteLine($"Pinging back {data} from {Name}");
        }
    }
}
