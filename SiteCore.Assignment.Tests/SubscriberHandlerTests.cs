using System.Linq;
using EventAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SiteCore.Assignment.Tests
{
    [TestClass]
    public class SubscriberHandlerTests
    {
        private SubscriberHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            var eventAggregator = new Mock<IEventAggregator>();
            _handler = new SubscriberHandler(eventAggregator.Object);
        }

        [TestMethod]
        public void AddSubscriber_ShouldHaveCorrectNumberofSubscibers()
        {
            // arrange
            // act
            _handler.AddSubscriber<int>();
            _handler.AddSubscriber<string>();
            _handler.AddSubscriber<int>();

            // assert
            Assert.IsTrue(_handler.Subscribers.Count(s => s.GetType() == typeof (PingSubscriber)) == 1);
            Assert.IsTrue(_handler.Subscribers.Count(s => s.GetType() == typeof(SumSubscriber)) == 2);
        }

        [TestMethod]
        public void Unsubscribe_ShouldUnsubscribeCorrectly()
        {
            // arrange
            var firstSub = _handler.AddSubscriber<int>();
            var secondSub = _handler.AddSubscriber<int>();

            // act
            _handler.Unsubscribe(secondSub.Name);

            // assert
            Assert.IsFalse(_handler.Subscribers
                .First(s => s.Name == secondSub.Name).IsLive);
            Assert.IsTrue(_handler.Subscribers
                .First(s => s.Name == firstSub.Name).IsLive);
        }

        [TestMethod]
        public void DeleteSubscriber()
        {
            // arrange
            var firstSub = _handler.AddSubscriber<int>();
            var secondSub = _handler.AddSubscriber<int>();

            // act
            _handler.DeleteSubscriber(secondSub.Name);

            // assert
            Assert.IsFalse(_handler.Subscribers
                .Any(s => s.Name == secondSub.Name));
        }
    }
}
