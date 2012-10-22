using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Tests
{
    public class Blog
    {
        public string Title { get; set; }
        public DateTime Starts { get; set; }
        public bool IsPublished { get; set; }
        public double Rating { get; set; }
    }



    public class Person
    {
        static Person()
        {
            Company = "CodeHelix";
        }


        public static string Company { get; set; }


        public Person()
        {
            Init("john", "doe", "johndoe@email.com", true, 10.5, DateTime.Now);
        }


        public Person(string first, string last)
        {
            Init(first, last, first + last + "@email.com", true, 10.5, DateTime.Now);
        }


        public Person(string first, string last, string email, bool isMale, double salary)
        {
            Init(first, last, email, isMale, salary, new DateTime(1980, 1, 2));
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
            if (birthday != DateTime.MinValue)
                BirthDate = birthday;
            else
                BirthDate = new DateTime(1980, 1, 2);
            Address = new Address() { City = "Queens", State = "NY" };
        }


        

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public bool IsMale { get; set; }
        public double Salary { get; set; }
        public Address Address { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }


        public string FullNameWithPrefix(string prefix, bool isSenior)
        {
            string fullname = string.IsNullOrEmpty(prefix) ? "" : prefix + " ";
            fullname += FirstName + " " + LastName;
            if (isSenior)
                fullname += " sr.";
            return fullname;
        }


        public bool SampleMethod(int dollers, double rating, bool isActive, string alias, DateTime postDate)
        {
            return true;
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
    }
}
