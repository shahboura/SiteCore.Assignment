using System;
using System.Collections.Generic;
using System.Linq;
using EventAggregator;

namespace SiteCore.Assignment
{
    class Program
    { 
        static void Main(string[] args)
        {
            IEventAggregator eventAggregator = new EventAggregator.EventAggregator();
            ISubscriberHandler subscriberHandler = new SubscriberHandler(eventAggregator);
            var strategies = LoadStrategies(eventAggregator, subscriberHandler);

            DisplayHelp();
            var input = Console.ReadLine();
            while (input != null && !input.Equals("q"))
            {
                // perform operations here
                var strategy = strategies.FirstOrDefault(s => s.IsCurrent(input));
                strategy?.PerformCommand();

                Console.WriteLine("==============================================");
                DisplayHelp();
                input = Console.ReadLine();
            }
        }

        public static List<Strategy> LoadStrategies(IEventAggregator eventAggregator, ISubscriberHandler handler)
        {
            return new List<Strategy>
            {
                new Strategy(s => s.Equals("i"), s => handler.AddSubscriber<int>()),
                new Strategy(s => s.Equals("s"), s => handler.AddSubscriber<string>()),
                new Strategy(s => s.Equals("a"), s => handler.DisplaySubscribers()),
                new Strategy(s => s.StartsWith("us "), s => handler.Unsubscribe(s.Remove(0, s.IndexOf(' ') + 1))),
                new Strategy(s => s.StartsWith("ss "), s => handler.Subscribe(s.Remove(0, s.IndexOf(' ') + 1))),
                new Strategy(s => s.StartsWith("d "), s => handler.DeleteSubscriber(s.Remove(0, s.IndexOf(' ') + 1))),
                new Strategy(s => s.StartsWith("p "), s =>
                {
                    var input = s.Remove(0, s.IndexOf(' ') + 1);
                    var output = 0;

                    if (int.TryParse(input, out output))
                    {
                        eventAggregator.Publish(output);
                    }
                    else
                    {
                        eventAggregator.Publish(input);
                    }
                })
            };
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Enter any of the following commands");
            Console.WriteLine("i: new sum subscriber\ts: new ping subscriber");
            Console.WriteLine("us {name}: unsubscribe by name\tss {name}: subscribe by name");
            Console.WriteLine("p {input}: publish int or string\ta: display all subscribers");
            Console.WriteLine("d {name} delete by name\tq: quit");
        }
    }
}
