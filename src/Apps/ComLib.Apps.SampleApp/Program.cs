using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ComLib.Application;
using CommonLibrary.Tests;
using ComLib.Lang.Helpers;
using ComLib.Lang.Tests.Unit;
using ComLib.Lang.Tests.Integration;
using ComLib.Lang.Tests.Integration.System;
using ComLib.Lang.Tests.Component;
using ComLib.Lang.Tests.Templating;

using ComLib.Lang;
using ComLib.Lang.AST;


namespace ComLib.Samples
{
    public class Complex
    {
        public Complex(int val) { Val = val; }

        public int Val;

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Val + b.Val);
        }
    }


    public class SampleAppRunner
    {        
        /// <summary>
        /// Sample application runner.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Combinator_Unit_Tests();
            Combinator_Integration_Tests();
            System_Control_Flow();
            Fluent_Tests();
            System_CSharp(); 
            Lang_Type_Tests();
            Combinator_Failures();
            
        }


        /// <summary>
        /// Prints to the console.
        /// </summary>
        /// /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        /// <param name="printline">Whether to print with line or no line</param>
        public static string Print(FunctionCallExpr exp)
        {
            string message = LogHelper.BuildMessage(exp.ParamList);
            Console.WriteLine(message);
            return message;
        }


        static void Lang_Printing()
        {
            var interpreter = new Interpreter();
            interpreter.Execute("var result = ['a', true, false, 123, 4.56, \"comlib\"];");
            interpreter.Execute(  "printline('helloworld');"
                                + "printline('hi:{0} last:{1}, age:{2}, is active:{3}', 'kishore', 'reddy', 32,   true);"
                                + "printline('hi:{0} last:{1}, age:{2}, is active:{3}', 'kishore', 'reddy', 32.5, false);");
            interpreter.Execute(  "log.info ('helloworld');"  
                                + "log.debug('Name:{0}, Age:{1}, IsActive:{2}', 'kishore', 32, true);"
                                + "log.warn ('helloworld');"
                                + "log.error('helloworld');"
                                + "log.fatal('helloworld');");
        }


        static void Lang_TokenizationTests()
        {
            var tests = new List<Tuple<string, string>>()
            {
                /*new Tuple<string, string>("if",       "var result = 1; if( 2 < 3 && 4 > 3 ) result = 1;"),
                new Tuple<string, string>("function", "var result = 1; function add(a) { return a + 1; } result = add(1);"),
                new Tuple<string, string>("logging",  "log.info ('helloworld');"),
                new Tuple<string, string>("arrays",   "var result = ['a', true, false, 123, 4.56, \"comlib\"];"),
                new Tuple<string, string>("maps",     "var result = { Name: 'user01', IsActive: true, IsAdmin: false, Id: 123, Sales: 45.6, Company: 'company.com' };"),
                */new Tuple<string, string>("callnest", "var result = 0; function inc(a) { return a + 1; }  result = inc(inc(1));")
            };

            foreach (var test in tests)
            {
                var interpreter = new Interpreter();
                interpreter.PrintTokens(test.Item2, @"c:\dev\tests\_lang\" + test.Item1 + "_tokens.csv");
                interpreter.PrintStatements(test.Item2, @"c:\dev\tests\_lang\" + test.Item1 + "_statements.csv");
                interpreter.PrintRunResult(@"c:\dev\tests\_lang\" + test.Item1 + ".csv");
            }
        }


        #region System Types
        static void Lang_Type_Tests()
        {
            System_Types_Dates();
            System_Types_Arrays();
            System_Types_Dictionary();
            System_Types_Time();
            System_Types_Strings();
        }


        static void System_Types_Arrays()
        {
            var t = new Types_Array();
            t.Can_Set_Array_ByIndex();
            // works
            t.Can_Do_Array_Declarations();
            t.Can_Get_Array_Basic_Type_Values_ByIndex();
            t.Can_Do_Array_Method_Calls();
            t.Can_Do_Array_Nested();
            t.Can_Get_Array_ByIndex();
            t.Can_Get_Array_Item_By_Index_Right_After_Declaration();
            t.Can_Get_Array_Item_By_Nested_Indexes();
            
        }


        static void System_Types_Strings()
        {
            var t = new Types_Strings();
            t.Can_Read_Interpolated_Strings();
            t.Can_Read_Interpolated_Strings_With_Custom_Interpolated_StartChar();
            t.Can_Do_String_Method_Calls();
            t.Can_Escape_Chars();
        }


        static void System_Types_Time()
        {
            var t = new Types_Time();
            t.Can_Declare_Time_With_New(); 
            t.Can_Add_Times();
        }


        static void System_Types_Dictionary()
        {
            var t = new Types_Dictionary();
            t.Can_Set_Values(); 
            t.Can_Declare(); 
            t.Can_Get_Values();
        }


        static void System_Types_Dates()
        {
            var t = new Types_Dates();
            t.Can_Get_Properties();
            t.Can_Set_Properties();
            t.Can_Subtract_Dates(); 
            t.Can_Create_Dates_With_Parameters();
            t.Can_Do_Date_Method_Calls();
        }
        #endregion


        #region System Control Flow
        static void System_Control_Flow()
        {
            System_Functions();
            System_Types();
            System_Limits();
            System_Errors_Runtime();
            System_TryCatch();
            System_Loops();
            System_Assignment();
        }


        static void System_Limits()
        {
            var t = new Script_Tests_Limits();
            t.Can_Set_Scope_Variables_String_Length_Total_Limit();
            t.Can_Set_Loop_Limit();
            t.Can_Set_Call_Stack_Cyclic_Limit();
            t.Can_Set_Exception_Limit();
            t.Can_Set_Function_Parameters_Limit();
            t.Can_Set_Loop_Limit_With_Try_Catch();
            t.Can_Set_Loop_Limit_With_Try_Catch_Then_Loop_Again();
        }


        static void System_Errors_Runtime()
        {
            var t = new Script_Tests_Errors_Runtime();
            t.Can_Handle_Index_Out_Of_Bounds();
            t.Can_Handle_Non_Existant_Map_Property();
            t.Can_Handle_Custom_Object_Non_Existant_Method();
            t.Can_Handle_Custom_Object_Non_Existant_Property();
            t.Can_Handle_Division_by_Zero();
            t.Can_Handle_Non_Existant_Function();
            t.Can_Handle_Variable_Not_Found();
        }


        static void System_TryCatch()
        {
            var t = new Script_Tests_TryCatch();
            t.Can_Do_Try_Catch();
        }

        static void System_Functions()
        {
            var t = new Script_Tests_Functions();
            t.Can_Ensure_Basic_Values_Are_Copied();
            t.Can_Make_Calls_With_Mixed_Types();
            t.Can_Define_With_Braces();
            t.Can_Define_With_Parameters_On_Separate_Lines();
            t.Can_Define_Without_Braces();
            t.Can_Have_Implicit_Arguments_Parameter();
            t.Can_Make_Calls_External_As_Expressions();
            t.Can_Make_Calls_External_As_Statements();
            t.Can_Make_Calls_Inside_External_Parenthesis();
            t.Can_Make_Calls_With_Extra_Parameters();
            t.Can_Make_Calls_Without_Parenthesis();
            t.Can_Make_Calls();
            t.Can_Return_With_Value();
            t.Can_Return_Without_Value();
            t.Can_Use_Aliases();
        }


        static void System_Assignment()
        {
            var t = new Script_Tests_Assignment();
            t.Can_Do_Unary_Expressions();
            t.Can_Do_Complex_Addition_On_Mixed_Types();
            t.Can_Do_Multiple_Assignment_Constant_Expressions_In_Same_Line();
            t.Can_Do_Multiple_Assignment_Expressions();
            t.Can_Do_Single_Assignment_Constant_Expressions();
            t.Can_Do_Single_Assignment_Constant_Logical_Expressions();
            t.Can_Do_Single_Assignment_Constant_Math_Expressions();
            t.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Mixed_Types();
            t.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence();
            t.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence_With_Parenthesis();
            t.Can_Handle_Escape_Chars_InString();
        }


        static void System_If()
        {
            var ti = new Script_Tests_If();
            ti.Can_Use_With_Constants();
            ti.Can_Use_With_Constants_Single_line();
            ti.Can_Use_Without_Parenthesis_With_Braces();
            ti.Can_Use_Without_Parenthesis_Without_Braces();
            ti.Can_Use_With_Parenthesis_With_Braces();
            ti.Can_Use_With_Parenthesis_Without_Braces();
            ti.Can_Use_Else_With_Constants();
            ti.Can_Use_Else_If_With_Constants();
        }


        static void System_Compare()
        {
            var t = new Script_Tests_Comparisons();
            t.Can_Do_Check_For_Nulls_Using_Complex_DataTypes();
            t.Can_Do_Check_For_Nulls_Using_Variables_Constants();
            t.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Bools();
            t.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Numbers();
            t.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Strings();
        }


        static void System_Loops()
        {
            var tl = new Script_Tests_Loops();
            tl.Can_Do_Recursion(); 
            tl.Can_Do_While_Statements_Syntax();
            tl.Can_Do_Nested_Loops();
            tl.Can_Do_While_Statements(); 
            tl.Can_Do_For_Each_Statements();
            tl.Can_Do_For_Loop_Statements();
            tl.Can_Do_Break_Statements();
        }


        static void System_Types()
        {
            var t = new Script_Tests_Types();
            t.Can_Do_Single_Assignment_New_Expressions(); 
            t.Can_Do_Check_For_Nulls();
            t.Can_Do_Check_For_Nulls2();
            t.Can_Do_Type_Changes();
        }
        #endregion


        #region CSharp
        static void System_CSharp()
        {
            System_CSharp_Objects(); 
            System_CSharp_Integration();
        }

        static void System_CSharp_Integration()
        {
            var t = new Script_Tests_CSharp_Integration();
            t.Can_Call_Function_Using_Dictionary();
            t.Can_Call_Function_Using_Different_Types_Of_Params1();
            t.Can_Call_Function_Using_Generic_List_Of_Basic_Types();
            t.Can_Call_Function_Using_Generic_List_Of_Objects();
            t.Can_Call_Function_With_Params();
            t.Can_Call_Function_Without_Params();
        }


        static void System_CSharp_Objects()
        {
            var t = new Script_Tests_CSharp_Objects();
            t.Can_Call_Instance_Methods_With_Named_Params();
            t.Can_Call_Instance_Methods_With_Named_Params_Using_Nulls();
            t.Can_Call_Static_Methods();
            t.Can_Get_Static_Properties();
            t.Can_Set_Static_Properties();
            
            t.Can_Set_Instance_Properties();
            t.Can_Get_Instance_Properties();
        }

        #endregion


        #region Combinators
        static void Combinator_Integration_Tests()
        {
            var t = new Plugin_Integration_Positives();
            t.Can_Use_Suffix();
            t.Can_Use_Units();
        }


        static void Combinator_Unit_Tests()
        {
            var t = new Plugin_Component_Positives();
            t.Can_Use_Time2_Plugin();
            t.Can_Use_TypeOf_Plugin();
            t.Can_Use_Env_Plugin();
            t.Can_Use_Exec_Plugin();
            t.Can_Use_Version_Plugin();
            t.Can_Use_TypeOps_Plugin();
            t.Can_Use_Sort_Plugin();
            t.Can_Use_Exec_Plugin();
            t.Can_Use_Log_Plugin();
            t.Can_Use_MachineInfo_Plugin();
            t.Can_Use_Day_Plugin();
            t.Can_Use_Fail_Plugin();
            t.Can_Use_TypeOps_Plugin();
            t.Can_Use_TypeOf_Plugin();
            t.Can_Use_Round_Plugin();
            t.Can_Use_Date_Plugin();
            t.Can_Use_DateNumber_Plugin();
            t.Can_Use_Aggregate_Plugin();
            t.Can_Use_Const_Plugin();
        }


        static void Combinator_Failures()
        {
            var t = new Plugin_Integration_Failures();
            t.Can_Access_Custom_Object_Instance_Properties();
        }
        #endregion


        #region Fluent
        static void Fluent_Tests()
        {
            var t = new Fluent_Call_Tests();
            t.Can_Handle_Ambiguity();
            t.Can_Use_Fluent_Member_Get_Property();
        }


        #endregion


        /// <summary>
        /// Sample application runner.
        /// Does pretty much the same thing as the above.
        /// But the above is shown just to display the API/Usage of ApplicationTemplate.
        /// </summary>
        /// <param name="args"></param>
        static void RunWithTemplateCall(IApp app, string[] args)
        {
            App.Run(app, args);
        }
    }
}
