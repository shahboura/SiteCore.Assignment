using System.Collections.Generic;

namespace SiteCore.Assignment
{
    public interface ISubscriberHandler
    {
        List<ISubscriber> Subscribers { get; }

        void DisplaySubscribers();
        ISubscriber AddSubscriber<T>();
        void Subscribe(string name);
        void Unsubscribe(string name);
        void DeleteSubscriber(string name);
    }
}
