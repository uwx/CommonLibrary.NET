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
            Combinator_Tests();
            //UnitTests();
            ExpressionTest();
            Lang_Script_Tests();
            TestVersion();
            FluentTests();
            Semantic_Tests();
            System_Control_Flow();
            Lang_Type_Tests(); 
            
        }


        static void ExpressionTest()
        {
            var e = Expression.Add(Expression.Constant(2), Expression.Constant("a"));
            
            var r = e.Reduce();
        }


        static void TestVersion()
        {
            var result1 = Convert.ChangeType("2.0", typeof(double), null);
            var result2 = Convert.ChangeType("8/15/2012", typeof(DateTime), null);
            
            Version v = new Version("0.9.8");
            Console.WriteLine(v.ToString());
        }


        static void ValidationTests()
        {
            var t=  new ValidationTests();
            t.Is_String_Length_Match();
            t.Is_String_Length_Match_With_Errors();
        }


        static void OperatorOverloadTest()
        {
            var c1 = new Complex(2);
            var c2 = new Complex(3);
            var c3 = c1 + c2;

            object o1 = c1, o2 = c2;
            var type = typeof(Complex);
            var all = type.GetMembers();
            var members = typeof(Complex).GetMember("op_Addition", BindingFlags.Public | BindingFlags.Static);

            var method = type.GetMethod("op_Addition", BindingFlags.Public | BindingFlags.Static);
            var o3 = method.Invoke(null, new object[] { o1, o2 });
            
           
            
        }


        static void Can_Do_Print()
        {
            var i = new Interpreter();
            i.Context.Settings.MaxLoopLimit = 10;
            i.Context.ExternalFunctions.Register("print", (exp) => Print(exp));
            i.Execute("print('testing {0}', 'kishore');");
        }


        /// <summary>
        /// Prints to the console.
        /// </summary>
        /// /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        /// <param name="printline">Whether to print with line or no line</param>
        public static string Print(FunctionCallExpr exp)
        {
            string message = FunctionHelper.BuildMessage(exp.ParamList);
            Console.WriteLine(message);
            return message;
        }


        static void RunLangTests()
        {
            string root = System.Environment.GetEnvironmentVariable("SYSTEMROOT");
            LangTestsAll();
        }


        static void TestDyn()
        {
            var p = new SampleAppRunner();
            var m = p.GetType().GetMethod("Concat");

            dynamic pd = p;
            pd.TryInvokeMember("c", null);
            
            Console.WriteLine(m.GetParameters().Length);
            Console.WriteLine(m.GetParameters()[0].GetType().FullName);
            Console.WriteLine(m.CallingConvention);
            Console.WriteLine(m);
        }


        public void Concat(params object[] items)
        {
            Console.WriteLine(items[0].ToString());
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


        static void TemplatingTests()
        {
            var tests = new Templating_Tests();
            tests.Can_Run();
        }



        static void FluentTests()
        {
            var tests = new Fluent_Call_Tests();
            tests.Can_Call_WildCard_With_Underscores();
            tests.Can_Call_MultiWord_Function_With_Underscores();
            tests.Can_Call_MultiWord_Function_With_CamelCasing();
            tests.Can_Use_Fluent_Member_Set_Property();
            tests.Can_Handle_Ambiguity();
            tests.Can_Call_Function_With_Named_Fluent_Parameters();
            tests.Can_Call_Function_With_Named_Parameters();
            tests.Can_Call_WildCard();
            tests.Can_Use_Fluent_Member_Set_Property();
            tests.Can_Use_Fluent_Member_Get_Property();
            tests.Can_Use_Fluent_Member_Method_Calls();            
        }

        /*
        static void UnitTests()
        {
            var ltJS = new Lang_LString2_Tests();

            var ltd = new Lang_LDate2_Tests();
            ltd.Can_Do_Date_SetMethods();
            ltd.Can_Do_Date_GetMethods();
            
            ltJS.Can_Test_Methods();
            ltJS.Can_Call_Execute();

            var ltJSa = new Lang_LArray2_Tests();
            ltJSa.Can_Test_Methods();

            var lexTests = new Lexer_Tests();
            lexTests.Can_Read_String(); 
            lexTests.Can_Read_Interpolated_Tokens();
            lexTests.Can_Read_Word();
            lexTests.Can_Read_Operator();
            lexTests.Can_Read_Symbol();
            lexTests.Can_Read_Number();            
            lexTests.Can_Read_Single_Line_Comment();
            lexTests.Can_Read_Multi_Line_Comment();
            
            lexTests.Can_Tokenize_Var_With_Multi_Line_Comment();
            lexTests.Can_Tokenize_Var_With_Single_Line_Comment();
            lexTests.Can_Tokenize_Var_On_Single_Line();
            lexTests.Can_Tokenize_Var_On_Multi_Line();
            lexTests.Can_Tokenize_Var_On_Multi_Line_With_Interpolated_Tokens();

            var tk = new TokenIteratorTests();
            tk.Can_Without_LLK_Init();
            tk.Can_With_LLK_Peek();
            tk.Can_With_LLK_Advance_To_MidPoint();
            tk.Can_With_LLK_Advance_To_NextBatch();
            tk.Can_With_LLK_Advance_N_Count();
            tk.Can_With_LLK_Advance_Once_Past_Many_Batches();
            tk.Can_With_LLK_Advance_To_End_Token();
                        
            
            var doc = new DocHelperTests();
            doc.Can_Parse_Doc_Tags_With_Named_Properties();
            doc.Can_Parse_Doc_Tags_With_Positional_Properties();

            var semactTests = new SemActs_Tests();
            semactTests.Can_Validate_Division_By_Zero();
            semactTests.Can_Validate_Variable_Does_Not_Exist();
             

            var symTests = new Symbols_Tests();
            var symTG = new Symbols_Global_Tests();
            symTG.Can_Get_Function_Symbol();
            symTests.Can_Have_Block_Inside_Function();

            var units = new UnitsTests();
            units.Can_Create_Units();
            units.Can_Convert_Values_Using_ShortName();
            units.Can_Register_Units();
            units.Can_Convert_Values();
            units.Can_Add_Values();


                       
            
            //var fcet = new FluentCallExpressionTests();
           // fcet.Can_Check_For_Matching_Multiword_Function_Name_In_Internal_Script();
           // fcet.Can_Check_For_Matching_Multiword_Function_Name_To_Single_Word_Function_In_Internal_Script();
           // fcet.Can_Check_For_Matching_Multiword_Function_Name_To_Single_Word_Underscore_Function_In_Internal_Script();
           // fcet.Can_Check_For_Matching_Multiword_Function_Name_In_External_Script();
           // 
        }
        */

        static void LangTestsAll()
        {
            Combinator_Tests();
            Lang_Script_Tests();
            Lang_Expression_Tests();
            //DataType_Tests();
            Lang_Scope_Tests();
        }


        static void RunScannerTests()
        {
            var t = new ScannerTests();
            t.CanParseNumber();
            t.CanParseId();
            t.CanParseString();
            t.CanParseLines();
            t.CanParseUntilChars();
            t.CanConsumeSpace();
        }


        static void Lang_Expression_Tests()
        {
            var t = new Expression_Tests();            
            //t.Can_Do_Array_Expressions();
            t.Can_Do_AssignmentExpressions();
            t.Can_Do_Compare_Expressions_On_Constants();
            t.Can_Do_Math_Expressions_On_Constants();
            t.Can_Do_Math_Expressions_On_Constants_And_Variables();
            t.Can_Do_Math_Expressions_On_Variables();            
            t.Can_Do_Unary_Operations();
        }


        static void Combinator_Tests()
        {
            Combinator_System_Tests();
            Combinator_Core_Tests();
            Combinator_Integration_Failures();
            Combinator_Integration_Tests();
            FluentTests();
            
        }


        static void Combinator_System_Tests()
        {
            var c = new Script_Tests_Assignment();
            var sb = new Script_Tests_Blocks();
            var ss = new Script_Tests_Syntax();
            var c2 = new Script_Tests_Comparisons();
            var sm = new Script_Tests_MemberAccess();
            var st = new Script_Tests_Types();
            var sf = new Script_Tests_Functions();

            
            var td = new Types_Dates();
            var ta = new Types_Array();

            c.Can_Handle_Escape_Chars_InString();
            c.Can_Do_Unary_Expressions();
            c.Can_Do_Single_Assignment_Constant_Logical_Expressions();
            c.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence();
            c.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence_With_Parenthesis();
            c.Can_Do_Single_Assignment_Constant_Math_Expressions();
            c.Can_Do_Single_Assignment_Constant_Expressions();
            
            c2.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Bools();
            c2.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Numbers();
            c2.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Strings();
                        
            c.Can_Do_Complex_Addition_On_Mixed_Types();
            c.Can_Do_Single_Assignment_Constant_Logical_Expressions();
            c.Can_Do_Multiple_Assignment_Expressions();
            c.Can_Do_Unary_Expressions();
            
            ta.Can_Get_Array_Item_By_Index_Right_After_Declaration();
            sf.Can_Make_Calls();
            sf.Can_Make_Calls_With_Extra_Parameters();
            sf.Can_Have_Implicit_Arguments_Parameter();
            
            ta.Can_Get_Array_Item_By_Nested_Indexes();
            sm.Can_Set_Class_Member_Property();
            td.Can_Create_Dates_With_Parameters();            
            ss.Can_Handle_SingleChar_NewLines();
            ss.Can_Handle_New_Lines_As_End_of_Stmt();
            sb.Can_Use_Non_Nested_BlockStatements();
            ta.Can_Get_Array_ByIndex();
            c2.Can_Do_Check_For_Nulls_Using_Complex_DataTypes();
            
        }



        static void Combinator_Integration_Failures()
        {
            var c = new Plugin_Integration_Failures();
            c.Can_Pass_Variables_To_Function_With_Same_ParameterNames();
            c.Can_Use_Email_Plugin_With_Incorrect_Chars();
            c.Can_Use_Bool_Plugin();
            c.Can_Use_Linq_Plugin_Inside_Parenthesis_In_If();
            c.Can_Access_Custom_Object_Instance_Properties();
            c.Can_Use_Function_In_If();
            c.Can_Use_Linq_Plugin_Inside_Parenthesis();
            c.Can_Use_NewLines_With_Fluent_Plugin();
        }


        static void Combinator_Integration_Tests()
        {
            var ci = new Plugin_Integration_Positives();
            ci.Can_Use_Units();
            ci.Can_Use_Repeat_Plugin();
            ci.Can_Use_Const_Plugin();
            ci.Can_Use_Linq_Plugin();
            ci.Can_Use_Suffix();
            ci.Can_Use_Sort_With_Set();
            ci.Can_Do_Unary_Expressions();
            ci.Can_Use_Compare_Plugin();
            ci.Can_Use_Day_And_Date_Plugins();
            ci.Can_Use_Word_Plugin();
            ci.Can_Use_Print_With_Space_With_Parenthesis();
            ci.Can_Use_New_Lines_As_End_of_Stmt();
            ci.Can_Use_Aggregates_In_Function_Call();
            ci.Can_Use_Date_Number_With_Times_Plugin();
            ci.Can_Use_Day_And_Date_Plugins();
            ci.Can_Use_Defect_With_Tables_Linq_Print();
            ci.Can_Use_Print_With_All_Plugins();
            ci.Can_Use_Variables_Named_With_Plugin_Keywords();
        }


        static void Combinator_Core_Tests()
        {            
            var c = new Plugin_Component_Positives();

            c.Can_Use_StringLiteral_Plugin();
            c.Can_Use_Exec_Plugin();
            c.Can_Use_IO_Plugin();
            c.Can_Use_Fail_Plugin();
            c.Can_Use_Version_Plugin();
            c.Can_Use_FileExt();
            c.Can_Use_Sort_Plugin();
            c.Can_Use_Env_Plugin();
            c.Can_Use_Marker_Plugin();
            c.Can_Use_Repeat_Plugin();
            c.Can_Use_Swap_Plugin();
            c.Can_Use_FileExt();
            c.Can_Use_TypeOps_Plugin();
            c.Can_Use_Env_Plugin();
            c.Can_Use_DateNumber_Plugin();
            c.Can_Use_Uri_Plugin();
            c.Can_Use_Email();
            c.Can_Use_Const_Plugin();
            c.Can_Use_TypeOf_Plugin();
            c.Can_Use_Repeat_Plugin();
            c.Can_Use_Const_Plugin();
            
            
            c._converter.Start();
            c.Can_Use_Alias_In_Script_Plugin();
            c.Can_Use_Def_Plugin();
            c.Can_Use_Set_Plugin();
            c.Can_Use_Time_Plugin();
            c.Can_Use_VariablePath_Plugin();
            c._converter.Finish();
            c._converter.WriteTo(@"C:\dev\business\CommonLibrary.NET\CommonLibraryNet_LATEST\src\Tests\tests.xml");

            c.Can_Use_Day_Plugin();
            c.Can_Use_Alias_In_Script_Plugin();
            c.Can_Use_Bool_Plugin();
            c.Can_Use_VariablePath_Plugin();
            c.Can_Use_Marker_Plugin();
            c.Can_Use_Percent_Plugin();
            c.Can_Use_Log_Plugin();
            c.Can_Use_MachineInfo_Plugin();
            c.Can_Use_HashComment_Plugin();
            c.Can_Use_Linq_Plugin_Using_Basic_Types();
            c.Can_Use_Word_Plugin();
            c.Can_Use_Sort_Plugin();
            c.Can_Use_Swap_Plugin();
            c.Can_Use_Compare_Plugin();
            c.Can_Use_Env_Plugin();
            c.Can_Use_Aggregate_Plugin();
            c.Can_Use_Uri_Plugin();            
            c.Can_Use_Takeover_Print_Plugin();
            c.Can_Use_Round_Plugin();
            c.Can_Use_Records_Plugin();
            c.Can_Use_Run_Plugin();
            c.Can_Use_Date_Plugin();
            c.Can_Use_Holiday_Plugin();
            c.Can_Use_Money_Plugin();
            c.Can_Use_Time_Plugin();          
        }

        /*
        static void DataType_Tests()
        {
            var t = new Lang_Custom_DataType_Tests();
            var s = new Lang_LString_Tests();
            var d = new Lang_LDate_Tests();
            var a = new Lang_LArray_Tests();
            var m = new Lang_LMap_Tests();
            
            
            d.Can_Do_Date_GetMethod_Tests();
            t.Can_Access_Member_Method_With_No_Args();
            a.Can_Do_Array_Expressions();
            d.Can_Do_Date_SetMethod_Tests();
            d.Can_Do_Date_ToStringMethod_Tests();
            s.Can_Do_String_Method_Tests();
            t.Can_Create_Types_Via_Context();
            t.Can_Create_Types_Via_NewExpression_ShortName();
            t.Can_Create_Types_Via_NewExpression_FullName();
            t.Can_Access_Custom_Object_Member_Properties();

            m.Can_Get_Property();
            m.Can_Set_Property();
        }
        */

        static void Lang_Scope_Tests()
        {
            var st = new ScopeTests();
            st.Can_Add_To_Default_Scope();
            st.Can_Pop_Scope();
            st.Can_Push_Scope();
            st.Can_Set_Same_Variable_Name_In_Different_Scopes();
            st.Can_Set_Value_From_Different_Scope();
        }


        static void Lang_Type_Tests()
        {
            var t = new Types_Dictionary();
            var ta = new Types_Array();
            var tt = new Types_Time();
            var td = new Types_Dates();
            var ts = new Types_Strings();

            ts.Can_Read_Interpolated_Strings();

            ts.Can_Escape_Chars();
            td.Can_Subtract_Dates();
            tt.Can_Declare_Time_With_New();
            ta.Can_Set_Array_ByIndex();
            ta.Can_Do_Array_Method_Calls();
            ta.Can_Get_Array_Item_By_Index_Right_After_Declaration();
            t.Can_Do_Map_Access();
            
        }



        static void System_Control_Flow()
        {

            var ti = new Script_Tests_If();
            ti.Can_Use_Without_Parenthesis_With_Braces(); 
            ti.Can_Use_Without_Parenthesis_Without_Braces();            
            ti.Can_Use_With_Parenthesis_With_Braces();
            ti.Can_Use_With_Parenthesis_Without_Braces();
            ti.Can_Use_With_Constants();
            ti.Can_Use_With_Constants_Single_line();
            ti.Can_Use_Else_With_Constants();
            ti.Can_Use_Else_If_With_Constants();


            var tl = new Script_Tests_Loops();
            tl.Can_Do_For_Each_Statements();
            tl.Can_Do_For_Loop_Statements();
            tl.Can_Do_Break_Statements();
            tl.Can_Do_Recursion();
            tl.Can_Do_While_Statements();
            tl.Can_Do_Nested_Loops();         
        }


        static void Lang_Script_Errors()
        {
            var ter = new Script_Tests_Errors_Runtime();
            var tes = new Script_Tests_Errors_Syntax();

            tes.Can_Handle_Unterminated_String();
            tes.Can_Handle_Unexpected_Char_At_Start();
            tes.Can_Handle_Multiple_Useless_Parenthesis();
            tes.Can_Handle_Double_Colon_On_Keys();
            tes.Can_Handle_Unterminated_String();
            tes.Can_Handle_Array_Syntax_Errors();
            tes.Can_Handle_Script_Syntax_Errors();

            ter.Can_Handle_Division_by_Zero();
            ter.Can_Handle_Custom_Object_Non_Existant_Method();
            ter.Can_Handle_Custom_Object_Non_Existant_Property();            
            ter.Can_Handle_Non_Existant_Function();
            ter.Can_Handle_Non_Existant_Map_Property();
            ter.Can_Handle_Index_Out_Of_Bounds();
            ter.Can_Handle_Division_by_Zero();

        }


        static void Semantic_Tests()
        {
            var s = new SemActs_Tests();
            s.Can_Validate_Division_By_Zero();
        }

        static void Lang_Script_Tests()
        {
            //Lang_Script_Errors();
            var ta = new Script_Tests_Assignment();
            var tf = new Script_Tests_Functions();
            
            var tt = new Script_Tests_Types();
            var tm = new Script_Tests_MemberAccess();
            var tlm = new Script_Tests_Limits();
            var tc = new Script_Tests_Comparisons();
            var tco = new Script_Tests_CustomObject();
            var ts = new Script_Tests_Syntax();
            var tcal = new Script_Tests_Callbacks();
            var tp = new Script_Tests_Parenthesis();
            var tmem = new Script_Tests_Memory();
            var tcs = new Script_Tests_CSharp_Integration();
            var tcb = new Script_Tests_Blocks();

            ta.Can_Do_Single_Assignment_Constant_Expressions();

            tcb.Can_Use_Non_Nested_BlockStatements();
                
            ta.Can_Do_Single_Assignment_Constant_Expressions();
            ta.Can_Do_Complex_Addition_On_Mixed_Types();
            ta.Can_Do_Single_Assignment_Constant_Math_Expressions();
            ta.Can_Do_Unary_Expressions();
            ta.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence();
            ta.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence_With_Parenthesis();
            ta.Can_Handle_Escape_Chars_InString();
            ta.Can_Do_Single_Assignment_Constant_Logical_Expressions();
            ta.Can_Do_Single_Assignment_Constant_Math_Expressions_With_Mixed_Types();
            ta.Can_Do_Multiple_Assignment_Expressions();
            ta.Can_Do_Multiple_Assignment_Constant_Expressions_In_Same_Line();
            

            tp.Can_Do_MultiLine_Conditions();

            tf.Can_Define_With_Parameters_On_Separate_Lines();            
            tf.Can_Define_With_Braces();
            tf.Can_Define_Without_Braces();
            
            tf.Can_Make_Calls_With_Extra_Parameters();
            tf.Can_Make_Calls_External_As_Expressions();
            tf.Can_Make_Calls_External_As_Statements();
            tf.Can_Make_Calls_With_Mixed_Types();
            tf.Can_Make_Calls_Inside_External_Parenthesis();
            tf.Can_Do_Try_Catch();
            tf.Can_Make_Calls();

            tcb.Can_Use_Try_Catch_With_Different_Syntaxes();

            tcs.Can_Call_Function_With_Params();
            tcs.Can_Call_Function_Without_Params();
            tcs.Can_Call_Function_Using_Different_Types_Of_Params1();
            tcs.Can_Call_Function_Using_Generic_List_Of_Basic_Types();
            tcs.Can_Call_Function_Using_Generic_List_Of_Objects();
            tcs.Can_Call_Function_Using_Dictionary();

            tt.Can_Do_Check_For_Nulls();

            tco.Can_Call_Custom_Object_Instance_Methods_With_Named_Params_Using_Nulls();
            tco.Can_Call_Custom_Object_Instance_Methods_With_Named_Params();
            tco.Can_Call_Custom_Object_Instance_Methods();
            tco.Can_Call_Custom_Object_Static_Methods();
            tco.Can_Create_Custom_Object_Via_Different_Constructors();
            tco.Can_Access_Custom_Object_Instance_Properties();
                                     
            

            tmem.Can_Pop_Memory();
            

            tp.Can_Do_Complex_Conditions();


            ta.Can_Do_Unary_Expressions();

            tp.Can_Start_With_Parenthesis();
            tp.Can_Do_Complex_Conditions();


           

            tt.Can_Do_Single_Assignment_New_Expressions();           
            

            ts.Can_Handle_Print_With_Space_With_Parenthesis();
            ts.Can_Handle_New_Lines_As_End_of_Stmt();
            ts.Can_Handle_SingleChar_NewLines();
            ts.Can_Do_Lexical_Replace();

            tcal.Can_Do_Statement_Callbacks();

            
            tc.Can_Do_Check_For_Nulls_Using_Complex_DataTypes();
            tc.Can_Do_Check_For_Nulls_Using_Variables_Constants();
            tc.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Bools();
            tc.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Numbers();
            tc.Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Strings();


            
            tm.Can_Get_Class_Member_Property();
            tm.Can_Set_Class_Member_Property();
            tm.Can_Get_Map_Member_Property();
            tm.Can_Set_Map_Member_Property();

            tlm.Can_Set_Exception_Limit();
            tlm.Can_Set_Call_Stack_Cyclic_Limit();
            tlm.Can_Set_Max_Expression_Limits();            
            tlm.Can_Set_Statement_Nested_Limit();
            tlm.Can_Set_Function_Parameters_Limit(); 
            tlm.Can_Set_Loop_Limit();
            tlm.Can_Set_Loop_Limit_With_Try_Catch();
            tlm.Can_Set_Loop_Limit_With_Try_Catch_Then_Loop_Again();
            tlm.Can_Set_Recursion_Limit();
            tlm.Can_Set_Max_Nested_Function_Calls_As_Arguments();
            tlm.Can_Set_Member_Expression_Limits();
            tlm.Can_Set_String_Limits();
            tlm.Can_Set_Scope_Variables_String_Length_Total_Limit();
        }


        static void RunAutomationTests()
        {
            var t = new AutomationTestsJs();
            t.CanCallFunc();
            t.CanParseFuncCallsUsingIndexPositions();
            t.CanParseFuncCallsUsingJsonNamedParameters();
        }


        static void Main2(string[] args)
        {
            IApp app = new Example_Scripting();
            try
            {       
                if (!app.Accept(args)) return;

                app.Init();
                app.Execute();
                app.ShutDown();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                Console.WriteLine("Error : " + ex.StackTrace);
            }

            Console.WriteLine("Finished... Press enter to exit.");
            Console.ReadKey();
        }


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
