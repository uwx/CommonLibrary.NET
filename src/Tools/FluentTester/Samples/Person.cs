using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Tools.FluentTester.Samples
{
    public class Person
    {
        public Person()
        {
            Init("john", "doe", "johndoe@email.com", true, 10.5, DateTime.Now.Date);
        }


        public Person(string first, string last)
        {
            Init(first, last, first + last + "@email.com", true, 10.5, DateTime.Now.Date);
        }


        public Person(string first, string last, string email, bool isMale, double salary)
        {
            Init(first, last, email, isMale, salary, DateTime.Now.Date);
        }


        public Person(string first, string last, string email, bool isMale, double salary, DateTime birthday)
        {
            Init(first, last, email, isMale, salary, birthday);
        }


        public void Init(string first, string last, string email, bool isMale, double salary, DateTime birthday)
        {
            FirstName = first;
            LastName = last;
            Email = email;
            IsMale = isMale;
            Salary = salary;
            BirthDate = birthday;
            Address = new Address() { City = "Queens", State = "NY" };
            FavoriteFoods = new List<string>();
            Friends = new List<Person>();
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public bool IsMale { get; set; }
        public bool IsActive { get; set; }
        public double Salary { get; set; }
        public Address Address { get; set; }
        public List<string> FavoriteFoods { get; set; }
        public List<Person> Friends { get; set; }


        public string FullName()
        {
            return FirstName + " " + LastName;
        }


        public void AddFavoriteFoodsList(List<string> foods)
        {            
            FavoriteFoods.AddRange(foods);
        }


        public void AddFriendsList(List<Person> friends)
        {
            Friends.AddRange(friends);
        }


        public string ChangeInfo(string first, string last, string email, bool isMale, double salary, DateTime birthDate)
        {
            FirstName = first;
            LastName = last;
            IsMale = isMale;
            Email = email;
            Salary = salary;
            BirthDate = birthDate;
            return email;
        }


        public string BuildContactInfo(string first, string last, bool isMale, DateTime startDate, double salary)
        {
            return first + ", " + last + ", " + isMale + ", " + startDate.Year + ", " + salary;
        }


        public static string ToFullName(string first, string last)
        {
            return first + " " + last;
        }
    }


    public class Address
    {
        public string City { get; set; }
        public string State { get; set; }


        public string FullLocation(int countryCode, bool useComma)
        {
            string location = "";
            if (useComma)
                location = City + ", " + State;
            else
                location = City + " " + State;
            if (countryCode == 1)
                location += " USA";
            else
                location += " CountryX";
            return location;
        }
    }
}
