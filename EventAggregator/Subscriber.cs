using System;

namespace EventAggregator
{
    internal class Subscriber
    {
        public WeakReference Context { get; set; }

        public Delegate Action { get; set; }
    }
}
