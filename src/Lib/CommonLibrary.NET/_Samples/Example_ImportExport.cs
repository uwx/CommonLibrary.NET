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
using ComLib.ImportExport;
//</doc:using>
using ComLib.Application;
using ComLib.MapperSupport;
using ComLib.ValidationSupport;



namespace ComLib.Samples
{
    /// <summary>
    /// Example for the ImportExport namespace.
    /// </summary>
    public class Example_ImportExport : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            //<doc:example>
            var io = ImportExports.Instance;
            
            // =======================================================================
            // REGISTRATION :
            // =======================================================================
            // Example 1.
            //  - Register Person as importable using default service, mappers, and formats as "csv,xml,ini"
            //  - Set the actions for the various import/export behaviour
            io.Register<Person>();
            io.For<Person>().SetImport((allPersons) => Console.WriteLine("Importing : " + allPersons.Count + " people."));
            io.For<Person>().SetExportPage( (page, pageSize) => GetPersons() );
            io.For<Person>().SetExportAll( () => GetPersons() );
            io.For<Person>().SetTotal(() => GetPersons().Count);

            // Example 2. 
            //  - Set the actions for the various import/export behaviour, ExportByPage lamda not supplied, exportbypage will call export all
            //  - Explicitly enable validation after mapping(using validator supplied), supported formats, and whether service is ReUsable ( e.g. singleton ).
            //  - .
            io.Register<Meeting>(new MeetingValidator(), new string[]{"csv", "ini", "xml"}, true );
            io.For<Meeting>().SetImport((allMeetings) => Console.WriteLine("Importing : " + allMeetings.Count + " meetings."));
            io.For<Meeting>().SetExportAll( () => GetMeetings() );
            io.For<Meeting>().SetTotal(() => GetMeetings().Count);

            // Example 3. 
            //  - Set the actions for the various import/export behaviour in a single call. import, exportbypage, exportall, gettotals.
            //  - Explicitly enable validation after mapping(using validator supplied), whether service is ReUsable ( e.g. singleton ), and mappers("ini","csv") to use.
            io.Register<BlogPost>(new MeetingValidator(), true, new MapperIni<BlogPost>(), new MapperCsv<BlogPost>());
            io.For<BlogPost>().SetHandlers((allPosts) => Console.WriteLine("Importing : " + allPosts.Count + " posts."),
                                            (page, pageSize) => GetBlogPosts(),
                                            () => GetBlogPosts(),
                                            () => GetBlogPosts().Count);

            // Example 4.
            //  - Use the Entity based importexport service to use EntityRepositories. No handlers need to be supplied.
            io.Register<BlogPost>(new ImportExportServiceEntityBased<BlogPost>());

            // IMPORT EXAMPLES
            // 1. Import by text.
            // 2. Import by File
            // 3. Import by supplying objects.
            var result1 = io.For<Person>().ImportText("", "ini");
            var result2 = io.For<Person>().ImportFile("../persons.ini");
            var result3 = io.For<Person>().Import(GetPersons());
            
            // TOTAL EXAMPLE
            // 1. How many are available for export ?
            var available = io.For<Meeting>().TotalExportable();

            // EXPORT EXAMPLES
            // 1. Export to a file
            // 2. Export as text.
            var export1 = io.For<Meeting>().ExportToFile("../meetings.csv", "csv");
            var export2 = io.For<Meeting>().ExportToText("csv", 1, 3);
            var export3 = io.For<Meeting>().Export(1, 3);

            //</doc:example>
            return BoolMessageItem.True;
        }



        private static IList<Person> GetPersons()
        {
            var persons = new List<Person>()
            {
                new Person(){ FirstName = "kishore", IsMale = true, BirthDate = DateTime.Today.AddYears(-30), Ssn = 123 },
                new Person(){ FirstName = "chris", IsMale = true, BirthDate = DateTime.Today.AddYears(-31), Ssn = 124 },
                new Person(){ FirstName = "jane", IsMale = false, BirthDate = DateTime.Today.AddYears(-32), Ssn = 125 },                     
            };
            return persons;
        }


        private static IList<Meeting> GetMeetings()
        {
            var meets = new List<Meeting>()
            {
                new Meeting(){ Title = "Discuss project", IsPublic = true, Date = DateTime.Today.AddDays(1), NumberOfSeats = 5 },
                new Meeting(){ Title = "Dev meeting", IsPublic = false, Date = DateTime.Today.AddDays(2), NumberOfSeats = 6 },
                new Meeting(){ Title = "Release Plan", IsPublic = true, Date = DateTime.Today.AddDays(3), NumberOfSeats = 7 },                     
            };
            return meets;
        }


        private static IList<BlogPost> GetBlogPosts()
        {
            var posts = new List<BlogPost>()
            {
                new BlogPost(){ Title = "Batman begins", IsPublic = true, Category = "movies" },
                new BlogPost(){ Title = "Scala Language", IsPublic = false, Category = "computer"},
                new BlogPost(){ Title = "Water Color Painting", IsPublic = true, Category = "art"},
            };
            return posts;
        }


        /// <summary>
        /// Sample class for import testing.
        /// </summary>
        private class Person
        {
            public string FirstName { get; set; }
            public bool IsMale { get; set; }
            public DateTime BirthDate { get; set; }
            public int Ssn { get; set; }
        }



        /// <summary>
        /// Sample class for import testing.
        /// </summary>
        private class Meeting
        {
            public string Title { get; set; }
            public bool IsPublic { get; set; }
            public DateTime Date { get; set; }
            public int NumberOfSeats { get; set; }
        }


        /// <summary>
        /// Sample class for import testing.
        /// </summary>
        private class BlogPost
        {
            public string Title { get; set; }
            public bool IsPublic { get; set; }
            public string Category { get; set; }
        }


        /// <summary>
        /// Example of a custom validator.
        /// </summary>
        private class MeetingValidator : Validator
        {

            /// <summary>
            /// Do some custom validation on a user name(string).
            /// </summary>
            /// <param name="validationEvent"></param>
            /// <returns></returns>
            protected override bool ValidateInternal(ValidationEvent validationEvent)
            {
                int originalErrorCount = validationEvent.Results.Count;

                Meeting meeting = validationEvent.TargetT<Meeting>();
                if (string.IsNullOrEmpty(meeting.Title))
                    validationEvent.Results.Add("Title must be specified.");

                return originalErrorCount == validationEvent.Results.Count;
            }
        }
    }
}
