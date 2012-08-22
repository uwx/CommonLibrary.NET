using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;

//<doc:using>
using ComLib;
using ComLib.Entities;
using ComLib.Extensions;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of ActiveRecord Initialization/Configuration.
    /// </summary>
    public class Example_ActiveRecord : App
    {
        private string _creationType = "factory_repo_only";

		//<doc:example>
        /// <summary>
        /// ActiveRecord type 2:
        /// Combined entity and active record functionality.
        /// </summary>
        private class Person : ActiveRecordBaseEntity<Person>
        {
            public string Name { get; set; }
            public string About { get; set; }
            public int Age { get; set; }
            public bool IsManager { get; set; }
            public DateTime StartDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Url { get; set; }
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            int lastId = 0;
			int sampleCount = 10;
			
            // 1. Create.
            sampleCount.Times( ndx =>
            {
                var newPerson = new Person()
                {
                    Name = "John", Age = ndx, Email = "john" + ndx + "@y.com",
                    IsManager = ndx % 2 == 0, StartDate = DateTime.Today.AddDays(-1 * ndx),
                    Phone = "111-111-111" + ndx, Url = "http://john" + ndx + ".com"
                };
                newPerson.Create();
                lastId = newPerson.Id;
            });

            // 2. Retrieve.
            Person person = Person.Get(lastId);
            Console.WriteLine("Retrieved {0}", person.Name);

            // 3. Update
            person.UpdateComment = "testing update";
            person.Update();
            Person afterUpdate = Person.Get(person.Id);
            Console.WriteLine(afterUpdate.UpdateComment);
                        
            // 4. Delete
            person.Delete();
            if (Person.Get(person.Id) == null)
                Console.WriteLine("deleted");

            // 6. Get All
            IList<Person> all = Person.GetAll();

            // 7. Get 1st Page
            PagedList<Person> page1 = Person.Get(1, 3);

            return BoolMessageItem.True;
        }
		//</doc:example>

        /// <summary>
        /// Configure the ActiveRecord for Accounts
        /// NOTES: - All service objects can be constructed with a repository, validator, settings object.            
        ///        - All service objects can be constructed with a fake repository ( in memory ) for testing.
        /// 
        /// POSSIBLE CONFIGURATIONS:
        /// 1. Singleton account service as is.            
        /// 2. Singleton account service and enable repository connection configuration.
        /// 3. Factory method for creating account service and enable repository connection configuration.
        /// 4. Factory methods for creating service, repository, validator and enabling repository connection configuration.
        /// </summary>
        /// <param name="context"></param>
        public override void Init(object context)
        {
            IRepository<Person> repository = new RepositoryInMemory<Person>();

            switch (_creationType)
            {
                case "factory_repo_only":
                    Person.Init(() => repository, false);
                    break;
                case "service_singleton":
                    Person.Init(new EntityService<Person>(repository, new EntityValidator(), new EntitySettings<Person>()));
                    break;
                case "service_singleton_db_config":
                    Person.Init(new EntityService<Person>(repository, new EntityValidator(), new EntitySettings<Person>()), false);
                    break;
                case "service_instance":
                    Person.Init(() => new EntityService<Person>(repository, new EntityValidator(), new EntitySettings<Person>()), false);
                    break;
                case "factory_all":
                    Person.Init(() => new EntityService<Person>(), () => new RepositoryInMemory<Person>(), () => new EntityValidator(), new EntitySettings<Person>(), false, null);
                    break;
                default:
                    Person.Init(() => new EntityService<Person>(), () => new RepositoryInMemory<Person>(), () => new EntityValidator(), new EntitySettings<Person>(), false, null);                    
                    break;
            }
        }
    }
}
