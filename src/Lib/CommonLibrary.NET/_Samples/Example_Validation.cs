using System;
using System.Collections.Generic;

using ComLib;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Application;
using ComLib.ValidationSupport;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the ValidationSupport namespace.
    /// </summary>
    public class Example_Validation : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_Validation()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("VALIDATION ");

            Example1_Static();
            Example2_StaticAndCollectErrors();
            Example3_Lamda();
            Example4_RulesList();
            Example5_Fluent();
            Example6_Custom();
            Example7_Chain();
            return BoolMessageItem.True;
        }


        private void Example1_Static()
        {
            // 1. EXAMPLE : Use static Valdiator class for simple validation.            
            Print("IsAlpha('123abc')          ", Validation.IsAlpha("123abc", false));
            Print("IsAlphaNum('123abc')       ", Validation.IsAlphaNumeric("123abc", false));
            Print("IsDate('08-29-2009')       ", Validation.IsDate("08-29-2009"));
            Print("IsNumeric('asdklf')        ", Validation.IsNumeric("asdklf"));
            Print("IsNumeric('-234.23')       ", Validation.IsNumeric("-234.23"));
            Print("IsPhoneUS('800-456-7890')  ", Validation.IsPhoneUS("800-456-7890", false));
            Print("IsSSN('123-45-7890')       ", Validation.IsSsn("123-45-7890", false));
            Print("IsTimeOfDay('7:45 am')     ", Validation.IsTimeOfDay("7:45 am"));
            Print("IsStringLengthMatch        ", Validation.IsStringLengthMatch("user01", false, true, true, 2, 12));
        }


        private void Example2_StaticAndCollectErrors()
        {
            // 2. EXAMPLE : Use Static Validator class and collect the errors.
            var errors = new Errors();
            Print("IsAlpha('123abc')          ", Validation.IsAlpha("123abc", false, errors, ""));
            Print("IsAlphaNum('123abc')       ", Validation.IsAlphaNumeric("123abc", false, errors, ""));
            Print("IsDate('08-29-2009')       ", Validation.IsDate("08-29-2009", errors, ""));
            Print("IsNumeric('asdklf')        ", Validation.IsNumeric("asdklf", false, errors, ""));
            Print("IsNumeric('-234.23')       ", Validation.IsNumeric("-234.23", false, errors, ""));
            Print("IsPhoneUS('800-456-7890')  ", Validation.IsPhoneUS("800-456-7890", false, errors, ""));
            Print("IsSSN('123-45-7890')       ", Validation.IsSsn("123-45-7890", false, errors, ""));
            Print("IsTimeOfDay('7:45am')      ", Validation.IsTimeOfDay("7:45am", errors, ""));
            Print("IsStringLengthMatch        ", Validation.IsStringLengthMatch("user01", false, true, true, 2, 12, errors, ""));
            PrintErrors(errors);
        }


        private void Example3_Lamda()
        {
            // 3. EXAMPLE : Use validator with lamda
            var val = new Validator(valEvent =>
            {
                int errCount = valEvent.Results.Count;
                Validation.IsEmail("kishore@", false, valEvent.Results, string.Empty);
                Validation.IsUrl("http://www", false, valEvent.Results, string.Empty);
                Validation.IsPhoneUS("111-111-111", false, valEvent.Results, string.Empty);

                return errCount == valEvent.Results.Count;
            });
            PrintErrors(val.Validate());
        }


        private void Example4_RulesList()
        {
            // 4. EXAMPLE: Validation w/ list of rules
            var val = new ValidatorWithRules();
            val.Add(e => Validation.IsEmail("kishore@", false, e.Results, string.Empty));
            val.Add(e => Validation.IsUrl("http://www", false, e.Results, string.Empty));
            val.Add(e => Validation.IsPhoneUS("111-111-111", false, e.Results, string.Empty));
            PrintErrors(val.Validate());            
        }


        private void Example5_Fluent()
        {
            // 5. EXAMPLE : Fluent validation.
            var val = new ValidatorFluent(typeof(User));
            var user = new User();
            val.Check(() => user.UserName).IsNotNull().IsBetween(1, 50)
               .Check(() => user.CreateDate).IsAfterToday()
               .Check(() => user.Email).IsValidEmail()
               .Check(() => user.MobilePhone).IsValidPhoneUS();

            PrintErrors(val.Errors);
        }


        private void Example6_Custom()
        {
            // 6. EXAMPLE : Use a custom validator.
            IValidator validator = new MyCustomUserIdValidator("admin");
            IValidationResults errors = new ValidationResults();

            // WAYS TO USE A VALIDATOR.
            // 1. Validate using the stored object(Target) with value("admin") and collect errors.
            // 2. Validate and collect errors into different error collection.
            // 3. Validate new target and collect errors.
            // 4. Validate new target and existing error collection.
            PrintErrors(validator.Validate());
            PrintErrors(validator.Validate(errors));
            PrintErrors(validator.ValidateTarget("powerUser01"));
            Print("Both", validator.Validate("powerUser01", errors));
        }


        private void Example7_Chain()
        {
            // 7. EXAMPLE : Chain multiple validators and validate all at once
            //              and store all the errors in a single error collection.
            var validators = new List<IValidator>()
            {
                new MyCustomUserIdValidator("admin"),
                new MyCustomUserIdValidator("batman")
            };

            // Run all the validators and collect the errors.
            ValidationResults errors = new ValidationResults();
            ValidationUtils.Validate(validators, errors);
        }


        /// <summary>
        /// Print the check that was done and it's result.
        /// </summary>
        /// <param name="checkName"></param>
        /// <param name="isValid"></param>
        private void Print(string checkName, bool isValid)
        {
            Console.WriteLine(checkName + " : " + isValid);
        }


        /// <summary>
        /// Print the errors.
        /// </summary>
        /// <param name="errors"></param>
        private void PrintErrors(IErrors errors)
        {
            string combinedErrors = errors.Message();
            Console.WriteLine("ERRORS: " + Environment.NewLine + combinedErrors);
        }
        

        /// <summary>
        /// Example of a custom validator.
        /// </summary>
        private class MyCustomUserIdValidator : Validator
        {
            /// <summary>
            /// Initialize the object to validate.
            /// </summary>
            /// <param name="userName"></param>
            public MyCustomUserIdValidator(string userName)
            {
                Target = userName;
            }


            /// <summary>
            /// Do some custom validation on a user name(string).
            /// </summary>
            /// <param name="validationEvent"></param>
            /// <returns></returns>
            protected override bool ValidateInternal(ValidationEvent validationEvent)
            {
                string id = (string)validationEvent.Target;
                if (string.IsNullOrEmpty(id))
                {
                    validationEvent.Results.Add("Must supply a userid.");
                    return false;
                }

                id = id.ToLower();
                if (id == "admin" || id == "administrator")
                {
                    validationEvent.Results.Add("Admin user name is reserved, you can not use it.");
                    return false;
                }

                if (id.Length < 2 || id.Length > 15)
                {
                    validationEvent.Results.Add("Must be between 2 >= username <= 15.");
                    return false;
                }
                return true;
            }
        }
    }
}
