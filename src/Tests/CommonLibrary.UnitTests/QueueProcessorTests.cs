using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Queue;

namespace CommonLibrary.Tests
{
   


    [TestFixture]
    public class QueueProcessorTests
    {

        [Test]
        public void CanProcessByType()
        {
            // Easiest way to add named processor for a specific type.
            // Defaults to 5 items per dequeue.
            Queues.AddProcessorFor<string>(items => ProcessString(items, true, null));
            Queues.Enqueue<string>("kdog");
            Queues.Enqueue<string>(new List<string>() { "kishore", "reddy" });
            Queues.Process<string>();

            Assert.IsTrue(Queues.IsIdle<string>());
        }


        [Test]
        public void CanProcessByAddingProcessor()
        {
            // Add the named processor "my_handler", w/ the ProcessString lamda, and handle 5 items per dequeue.
            Queues.AddProcessor("my_handler", new QueueProcessor<string>(5, items => ProcessString(items, false, "my_handler")));
            Queues.Enqueue<string>("my_handler", "kdog");
            Queues.Enqueue<string>("my_handler", new List<string>() { "kishore", "reddy" });
            Queues.Process("my_handler");

            Assert.IsTrue(Queues.IsIdle("my_handler"));
        }


        [Test]
        public void CanProcessByMultipleTimes()
        {
            // Easiest way to add named processor for a specific type.
            // Defaults to 5 items per dequeue.
            Queues.AddProcessorFor<string>(items => ProcessString(items, true, null));
            Queues.Enqueue<string>(new List<string>() { "kishore", "reddy", "kdog", "kishore", "reddy", "kdog" });
            Queues.Enqueue<string>(new List<string>() { "kdog", "reddy", "kishore", "kdog", "reddy", "kishore" });

            // Need to process multiple times since there are more than 5 entries which is the default 
            // number of items that get processed by each call to Process().
            // This can then be combined w/ the scheduler to perform some scheduled processing.
            // e.g.
            // Scheduler.Run( Every(1.Minute), () => Queues.Process<string>());

            Queues.Process<string>();
            Queues.Process<string>();

            Assert.IsTrue(Queues.IsIdle<string>());
        }


        [Test]
        public void CanGetQueueStates()
        {
            // Easiest way to add named processor for a specific type.
            // Defaults to 5 items per dequeue.
            Queues.AddProcessorFor<string>(items => ProcessString(items, true, null));
            Queues.AddProcessor("tags_queue", new QueueProcessor<string>(2, items => items.ForEach( item => Console.WriteLine(item))));
            
            Queues.Enqueue<string>("tags_queue", new List<string>() { "python", "ruby", "erlang", "closure", "scala" });
            Queues.Enqueue<string>(new List<string>() { "kishore", "reddy"  });
            
            Queues.Process<string>();
            Queues.Process("tags_queue");
            Queues.Process("tags_queue");

            var states = Queues.GetMetaInfo();
            var map = new Dictionary<string, QueueStatus>();
            foreach (var state in states)
                map[state.Name] = state;

            Assert.AreEqual(states.Count, 2);
            Assert.AreEqual(map["System.String"].Count, 0);
            Assert.AreEqual(map["System.String"].DequeueSize, 10);
            Assert.AreEqual(map["System.String"].NumberOfTimesProcessed, 1);
            Assert.AreEqual(map["System.String"].State, QueueProcessState.Idle);
            Assert.AreEqual(map["tags_queue"].Count, 1);
            Assert.AreEqual(map["tags_queue"].DequeueSize, 2);
            Assert.AreEqual(map["tags_queue"].NumberOfTimesProcessed, 2);
            Assert.AreEqual(map["tags_queue"].State, QueueProcessState.Idle);
        }


        public void ProcessString(IList<string> names, bool isGenericsApplicable, string handlerName)
        {
            Dictionary<string, bool> validValues = new Dictionary<string, bool>();
            validValues["kdog"] = true;
            validValues["kishore"] = true;
            validValues["reddy"] = true;

            if(names == null || names.Count == 0)
                return;

            if (isGenericsApplicable)
            {
                Assert.IsTrue(Queues.IsBusy<string>());
                Assert.IsTrue(Queues.IsBusy(typeof(string).FullName));
            }
            else
            {
                Assert.IsTrue(Queues.IsBusy(handlerName));
            }

            foreach (string name in names)
                Assert.IsTrue(validValues.ContainsKey(name));
        }        
    }
}
