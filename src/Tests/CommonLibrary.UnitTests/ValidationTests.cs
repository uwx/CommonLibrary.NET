using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ComLib;
using ComLib.ValidationSupport;
using NUnit.Framework;
using Val = System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Tests
{

    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void CanValidateUsingAnnotations()
        {
            var product = new Product("", "", false, 2, 1, DateTime.Now);
            product.Url = "incorrect";
            var errors = new Errors();
            bool result = Validation.ValidateObject(product, errors);
            Assert.IsFalse(result);
            Assert.IsTrue(errors.Count > 0);
        }


        [Test]
        public void Is_String_Length_Match()
        {
            var list = new[]
            { 
                new { Text = "",    Result = false, AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 5 },
                new { Text = "",    Result = true,  AllowNull = true,  CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 5 }, 
                new { Text = "ab",  Result = true,  AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 3, MaxLen = 5 },
                new { Text = "ab",  Result = false, AllowNull = false, CheckMinLen = true,  CheckMaxLen = false,  MinLen = 3, MaxLen = 5 }, 
                new { Text = "abc", Result = true,  AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 2 },
                new { Text = "abc", Result = false, AllowNull = false, CheckMinLen = false, CheckMaxLen = true,   MinLen = 1, MaxLen = 2 }
            };

            foreach (var item in list)
            {
                var result = Validation.IsStringLengthMatch(item.Text, item.AllowNull, item.CheckMinLen, item.CheckMaxLen, item.MinLen, item.MaxLen);
                Assert.AreEqual(item.Result, result);
            }
        }


        [Test]
        public void Is_String_Length_Match_With_Errors()
        {
            var list = new[]
            { 
                new { Text = "",    ParamName = "Name", ErrorCount = 1, Result = false, AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 5 },
                new { Text = "",    ParamName = "Name", ErrorCount = 0, Result = true,  AllowNull = true,  CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 5 }, 
                new { Text = "ab",  ParamName = "Name", ErrorCount = 0, Result = true,  AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 3, MaxLen = 5 },
                new { Text = "ab",  ParamName = "Name", ErrorCount = 1, Result = false, AllowNull = false, CheckMinLen = true,  CheckMaxLen = false,  MinLen = 3, MaxLen = 5 }, 
                new { Text = "abc", ParamName = "Name", ErrorCount = 0, Result = true,  AllowNull = false, CheckMinLen = false, CheckMaxLen = false,  MinLen = 1, MaxLen = 2 },
                new { Text = "abc", ParamName = "Name", ErrorCount = 1, Result = false, AllowNull = false, CheckMinLen = false, CheckMaxLen = true,   MinLen = 1, MaxLen = 2 }
            };

            foreach (var item in list)
            {
                var errors = new Errors();
                var result = Validation.IsStringLengthMatch(item.Text, item.AllowNull, item.CheckMinLen, item.CheckMaxLen, item.MinLen, item.MaxLen, errors, item.ParamName);
                Assert.AreEqual(item.Result, result);
                Assert.AreEqual(errors.Count, item.ErrorCount);
            }
        }


        [Test]
        public void Is_Phone_Valid()
        {
            ValidationResults errors = new ValidationResults();
            Assert.IsTrue(Validation.IsPhoneUS("347-512-3161", false, errors, null));
        }


        [Test]
        public void Is_Phone_Invalid()
        {
            ValidationResults errors = new ValidationResults();
            Assert.IsFalse(Validation.IsPhoneUS("347sdf-512-3161", false, errors, null));
        }


        [Test]
        public void Can_Validate_Phone()
        {
            ValidationResults errors = new ValidationResults();
            Assert.IsTrue(Validation.IsPhoneUS("3475123161", false, errors, null));
        }


        [Test]
        public void CanValidateFluently()
        {
            var product = new Product("MagicPencil", "draw like an artist", true, 10, 2, DateTime.Today.AddDays(5));
            var validator = new ValidatorFluent(product.GetType());

            validator.Check(product.Title).IsNotNull().IsBetween(1, 15)
                     .Check(product.Description).IsNot(string.Empty).IsBetween(1, 25)
                     .Check(product.Category).Min(1)
                     .Check(product.Cost).Min(1).Max(500)
                     .Check(product.IsInStock).IsTrue()
                     .Check(product.AvailableDate).IsAfterToday().End();

            Assert.IsFalse(validator.HasErrors);
        }



        private class Product 
        {
            [StringLength( 20, MinimumLength = 1)]
            public string Title { get; set; }

            [Required]            
            public string Description { get; set; }
            
            
            public bool IsInStock { get; set;}

            [System.ComponentModel.DataAnnotations.Range(5, 200000)]
            public double Cost { get; set; }

            
            [Required]
            public int Category { get; set; }
            

            public DateTime AvailableDate { get; set; }

            
            [RegularExpression(@"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$")]
            public string Url { get; set; }


            public Product(string title, string desc, bool inStock, double cost, int category, DateTime availableDate)
            {
                Title = title;
                Description = desc;
                IsInStock = inStock;
                Cost = cost;
                Category = category;
                AvailableDate = availableDate;
            }
        }
    }
}
