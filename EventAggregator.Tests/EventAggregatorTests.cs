using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventAggregator.Tests
{
    [TestClass]
    public class EventAggregatorTests
    {
        private IEventAggregator _aggregator;

        [TestInitialize]
        public void TestInitialize()
        {
            _aggregator = new EventAggregator();
        }

        [TestMethod]
        public void Publish_InvokeAllRegisteredSubscribers()
        {
            // arrange
            var count = 0;

            // act
            _aggregator.Subscribe<int>(new object(), d => count++);
            _aggregator.Subscribe<int>(new object(), d => count++);
            _aggregator.Publish(10);

            // assert
            Assert.AreEqual(2, count);
        }
        
        [TestMethod]
        public void Publish_InvokeByType()
        {
            // arrange
            var count = 0;
            var message = string.Empty;
            var context = new object();

            // act
            _aggregator.Subscribe<int>(context, d => count++);
            _aggregator.Subscribe<int>(context, d => count++);
            _aggregator.Subscribe<string>(context, d => message = d);

            _aggregator.Publish("hello from the other side");

            // assert
            Assert.AreEqual(0, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Publish_InactiveContextRemovedBeforeCall()
        {
            // arrange
            var count = 0;
            var doomedContext = new object();
            var liveContext = new object();

            // act
            _aggregator.Subscribe<int>(doomedContext, d => count++);
            _aggregator.Subscribe<int>(liveContext, d => count++);

            doomedContext = null;
            GC.Collect();

            _aggregator.Publish(10);

            // assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void Publish_DataPassedCorrectlyToSubscribers()
        {
            // arrange
            var count = 0;
            var message = string.Empty;

            // act
            _aggregator.Subscribe<int>(new object(), d => count = d);
            _aggregator.Subscribe<string>(new object(), d => message = d);
            
            _aggregator.Publish(20);
            _aggregator.Publish("hello from the other side");

            // assert
            Assert.AreEqual(20, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Unsubscribe_ByType_RemovedSuccessfully()
        {
            // arrange
            var count = 0;
            var message = string.Empty;

            // act
            _aggregator.Subscribe<int>(new object(), d => count = d);
            _aggregator.Subscribe<string>(new object(), d => message = d);

            _aggregator.Unsubscribe<int>();

            _aggregator.Publish(20);
            _aggregator.Publish("hello from the other side");

            // assert
            Assert.AreEqual(0, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Unsubscribe_ByContext_RemovedSuccesfully()
        {
            // arrange
            var count = 0;
            var message = string.Empty;
            var subContext = new object();
            var unsubContext = new object();

            // act
            _aggregator.Subscribe<int>(subContext, d => count++);
            _aggregator.Subscribe<int>(unsubContext, d => count++);
            _aggregator.Subscribe<string>(unsubContext, d => message = d);

            _aggregator.Unsubscribe(unsubContext);

            _aggregator.Publish(10);
            _aggregator.Publish("hello from the other side");

            // assert
            Assert.AreEqual(1, count);
            Assert.AreEqual(string.Empty, message);
        }

        [TestMethod]
        public void Unsubscribe_ByContextAndType_RemovedSuccesfully()
        {
            // arrange
            var count = 0;
            var message = string.Empty;
            var subContext = new object();
            var unsubContext = new object();

            // act
            _aggregator.Subscribe<int>(subContext, d => count++);
            _aggregator.Subscribe<int>(unsubContext, d => count++);
            _aggregator.Subscribe<string>(unsubContext, d => message = d);

            _aggregator.Unsubscribe<int>(unsubContext);

            _aggregator.Publish(10);
            _aggregator.Publish("hello from the other side");

            // assert
            Assert.AreEqual(1, count);
            Assert.AreEqual("hello from the other side", message);
        }
    }
}
