using System;
using EventAggregator;

namespace SiteCore.Assignment
{
    public class SumSubscriber : Subscriber<int>
    {
        public SumSubscriber(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public override void Action(int data)
        {
            Data += data;
            Console.WriteLine($"Sum is {Data} from {Name}");
        }
    }
}
