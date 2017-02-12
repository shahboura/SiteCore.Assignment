using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class SumSubscriber : Subscriber<int>
    {
        public SumSubscriber(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Action = s =>
            {
                Data += s;
                Console.WriteLine($"Sum is {s} from {Name}");
            };
        }
    }
}
