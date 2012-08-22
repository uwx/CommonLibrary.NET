using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;

using ComLib.Patterns;


namespace CommonLibrary.Tests
{
    public interface IWorker
    {
        string Name { get; set; }
        string About();
    }

    public abstract class Person
    {
        public string Name { get; set; }
        public abstract string About();
    }


    public class Contractor : Person, IWorker
    {
        public override string About() { return "contractor : " + Name; }
    }


    public class Employee: Person, IWorker
    {
        public override string About() { return "fulltime : " + Name; }
    }


    [TestFixture]
    public class PatternsTests
    {
        [Test]
        public void CanCreateFactory()
        {
            Factory<string, IWorker>.Register("c1",  new Contractor() { Name = "c1" });
            Factory<string, IWorker>.Register("c2", new Contractor() { Name = "c2" });
            Factory<string, IWorker>.Register("e1",  new Employee() { Name = "e1" });
            Factory<string, IWorker>.Register("e2", new Employee() { Name = "e2" });
            Assert.AreEqual(Factory<string, IWorker>.Create("c1").Name, "c1");
            Assert.AreEqual(Factory<string, IWorker>.Create("c2").Name, "c2");
            Assert.AreEqual(Factory<string, IWorker>.Create("e1").Name, "e1");
            Assert.AreEqual(Factory<string, IWorker>.Create("e2").Name, "e2");
        }
    }
}
