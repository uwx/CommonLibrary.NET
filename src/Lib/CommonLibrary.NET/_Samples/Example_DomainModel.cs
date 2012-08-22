using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

using ComLib.Entities;
using ComLib.Account;
using ComLib;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Entities namespace.
    /// </summary>
    public class Example_DomainModel : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_DomainModel()
        {
        }
        

        /// <summary>
        /// Person domain object.
        /// </summary>
        public class Person : Entity
        {
            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }


            /// <summary>
            /// Social security.
            /// </summary>
            public int Ssn { get; set; }


            /// <summary>
            /// some description.
            /// </summary>
            public string PersonDescription { get; set; }
        }



        /// <summary>
        /// Validator class for the Person domain object.
        /// </summary>
        public class PersonValidator : EntityValidator
        {
            /// <summary>
            /// Validates a Person object.
            /// </summary>
            /// <param name="validationEvent">Event to use.</param>
            /// <returns>Validation result.</returns>
            protected override bool ValidateInternal(ValidationEvent validationEvent)
            {
                Person p = validationEvent.Target as Person;
                if (string.IsNullOrEmpty(p.Name))
                {
                    validationEvent.Results.Add("Name is not supplied.");
                    return false;
                }
                else if (p.Name.Length > ((PersonSettings)p.Settings).MaxLength)
                {
                    validationEvent.Results.Add("Name exceeds max lenght.");
                    return false;
                }
                return true;
            }
        }



        /// <summary>
        /// Settings class for the person domain object.
        /// </summary>
        public class PersonSettings : EntitySettings<Person>
        {
            /// <summary>
            /// Max length for persons name.
            /// </summary>
            public int MaxLength { get; set; }


            /// <summary>
            /// Default the values by overriding the initialization.
            /// </summary>
            public override void Init()
            {
                EnableValidation = true;
                MaxLength = 8;
            }
        }



        /// <summary>
        /// Class to use the ActiveRecord behaviour.
        /// </summary>
        public class Persons : ActiveRecordBase<Person>
        {
            /// <summary>
            /// Creates a new instance of BlogPost and 
            /// initializes it with a validator and settings.
            /// </summary>
            /// <returns></returns>
            public static Person New()
            {
                Person entity = new Person();

                // Can subclass but just used for 
                // Settings could otentially be a singleton. Up to the developer to decide.
                entity.Settings = new PersonSettings();
                return entity;
            }
        }



        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // Fake repository for examples/unit testing.
            // Use this since database is not ready.
            IRepository<Person> repo = new RepositoryInMemory<Person>("Id,Name,Ssn");

            // 1. Initialize the configuration for active record.
            // See Example_ActiveRecord for more information.
            Persons.Init(() => new EntityService<Person>(repo, new PersonValidator(), new PersonSettings()), false);

            // 2. Create new Person
            Person p = Persons.New();
            p.Name = "kishore";
            p.Ssn = 123456789;
                        
            // 3. Save.
            Persons.Save(p);
            Console.WriteLine("saved");

            // 4. Retrieve by id.
            Person p2 = Persons.Get(p.Id);
            Console.WriteLine("Name : {0} - Ssn {1} - Desc {2}", p2.Name, p2.Ssn, p2.PersonDescription);

            return BoolMessageItem.True;
        }
    }
}
