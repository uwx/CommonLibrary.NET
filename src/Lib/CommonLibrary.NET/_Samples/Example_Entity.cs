using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

//<doc:using>
using ComLib;
using ComLib.Entities;
using ComLib.ValidationSupport;
using ComLib.Application;
//</doc:using>
using ComLib.Account;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Entities namespace.
    /// </summary>
    public class Example_Entity : App
    {
		//<doc:example>
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("Entity ");

            // Example 1 : Using Entity base class.
            IEntity entity = new Person() { Name = "kishore" };
            Console.WriteLine("Id, Persistant and Audit fields.");
            Console.WriteLine("These fields are set by EntityService if using CommonLibrary Entity-Model Services.");
            Console.WriteLine("Id            ", entity.Id);
            Console.WriteLine("IsPersistant  ", entity.IsPersistant());            
            Console.WriteLine("CreateDate    ", entity.CreateDate);
            Console.WriteLine("CreateUser    ", entity.CreateUser);
            Console.WriteLine("UpdateDate    ", entity.UpdateDate);
            Console.WriteLine("UpdateUser    ", entity.UpdateUser);
            Console.WriteLine("UpdateComment ", entity.UpdateComment);
            
            // Example 2 : Using Entity<T> generic base class to get the parameterless CRUD methods.
            // 1st initialize the repositories.
            EntityRegistration.Register<Person2>(new RepositoryInMemory<Person2>("Id"), false);
            var p2 = new Person2() { Name = "kishore" };
            p2.Create();
            p2.Name = "updated";
            p2.Update();
            p2.GetAll();
            p2.Save();
            p2.Delete();
            
            // Example 3 : Implement IEntityVersionable to get versioning on the entities.
            // Wrap the real repository w/ the RepositoryVersionable decorator to get versioning behaviour.
            EntityRegistration.Register<Person3>(new RepositoryVersioned<Person3>(new RepositoryInMemory<Person3>("Id,Version,VersionRefId")), false);
            var p3 = new Person3();
            p3.Create();
            p3.Name = "updated";
            p3.Update();
            p3.GetAll();

            var p4 = new Person4();
            p4.Name = "kishore";
            p4.Phone = "111-111-1111";
            p4.RegisterDate = DateTime.Today;
            p4.About = "artist";
            p4.Age = 30;
            p4.Email = "kishore@ny.com";
            bool isValid = p4.IsValid;
            return BoolMessageItem.True;
        }


        /// <summary>
        /// This class just show how to subclass the Entity base class
        /// to get various built-in properties like Id, CreateDate etc.
        /// </summary>
        private class Person : Entity
        {
            public string Name { get; set; }
        }



        /// <summary>
        /// This class shows how to subclass the generics based Entity(T)
        /// base class to get parameterless CRUD ( Create, Get, Update, Delete, All ) methods.
        /// </summary>
        private class Person2 : Entity<Person2>
        {
            public string Name { get; set; }
        }



        /// <summary>
        /// This class shows how to implement the IEntityVersioned interface to get
        /// the entities to be versioned in your repository.
        /// </summary>
        private class Person3 : Entity<Person3>, IEntityVersioned
        {
            public string Name { get; set; }

            /// <summary>
            /// Current version
            /// </summary>
            public int Version { get; set; }


            /// <summary>
            /// Reference to the Id ( primary key ) of the latest version.
            /// This is > 0 for older/historic versions of the entity.
            /// The latest version will always have this set to -1.
            /// </summary>
            public int VersionRefId { get; set; }


            /// <summary>
            /// Is this the latest version?
            /// </summary>
            /// <returns></returns>
            public bool IsLatestVersion() { return VersionRefId == -1; }
        }



        private class Person4 : Entity<Person4>
        {
            public string Name { get; set; }
            public string About { get; set; }
            public int Age { get; set; }
            public bool IsLegalAge { get { return Age >= 21; } }
            public DateTime RegisterDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Url { get; set; }


            protected override IValidator GetValidator()
            {
                var val = new Validator((valEvent) =>
                {
                    var errors = valEvent.Results;
                    var validator = new ValidatorFluent(this.GetType(), true, errors);
                    validator.Check(() => Name).IsNotNull().IsBetween(1, 50)
                             .Check(() => Age).Min(18).Max(65)
                             .Check(() => About).If(!string.IsNullOrEmpty(About)).IsBetween(1, 20)
                             .Check(() => IsLegalAge).IsTrue()
                             .Check(() => RegisterDate).IsAfterToday()
                             .Check(() => Email).IsValidEmail()
                             .Check(() => Phone).IsValidPhoneUS()
                             .Check(() => Url).IsValidUrl();

                    return validator.HasErrors;
                });
                val.Validate();
                return null;
            }
        }
		//</doc:example>
    }
}
