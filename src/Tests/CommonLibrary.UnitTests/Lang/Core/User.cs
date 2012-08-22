using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Tests.Common
{
    /*
         *  Case 1. func                                        notify
         *  Case 2. func parameter                              notify          'group1'         
         *  Case 3. func parameter parameter			        print 	        'kishore' 			'reddy'
         *  Case 4. class  method 		                        user  	        IsRegistered 	
         *  Case 5. method class  		                        IsRegistered    user
         *  Case 6. class parameter method                      user            'kreddy'            exists
         *  Case 7. method parameter class			            activate        'admin'		        User
         *  Case 8. class methodPart methodPart [parameter]     user            has                 savings account
         *  Case 9. methodPart class methodPart [parameter]	    add 	        user				to role 'admin', 'division2'
        */
    public class User
    {

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public double Salary { get; set; }


        public User(string name)
        {
            Name = name;
        }


        public bool IsRegistered()
        {
            return true;
        }


        public bool HasSavingsAccount()
        {
            return true;
        }


        public bool AddToRole(string role, string division)
        {
            return true;
        }


        public static bool Activate(string user)
        {
            return true;
        }


        public static bool Exists(string name)
        {
            return string.Compare(name, "kreddy", true) == 0;
        }


        public static User Create(string name, bool isActive, DateTime bday, double salary)
        {
            var user = new User(name);
            user.BirthDate = bday;
            user.IsActive = isActive;
            user.Salary = salary;
            return user;
        }
    }
}
