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
using ComLib.Data;
using ComLib.Models;
using ComLib.CodeGeneration;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the CodeGeneration namespace.
    /// </summary>
    public class Example_CodeGenerator : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_CodeGenerator()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            ModelContainer models = GetModelContainer();
            ModelContext ctx = new ModelContext() { AllModels = models };
            IList<ICodeBuilder> builders = new List<ICodeBuilder>()
            {
                // This generates the Database tables in SQL - Server.
                // You connection string in ModelBuilderSettings.Connection must be set.
                new CodeBuilderDb(ctx.AllModels.Settings.Connection),
                new CodeBuilderDomain(),              
            };
            BoolMessage message = CodeBuilder.Process(ctx, builders);
            Console.WriteLine("Code generation Sucess : {0} - {1}", message.Success, message.Message);
            return BoolMessageItem.True;
        }


		
        private ModelContainer GetModelContainer()
        {
            var baseLocComLib = @"F:\Business\CommonLibrary.NET\CommonLibraryNet.LATEST\";
            var baseLocCode = @"F:\Business\CommonLibrary.NET\CommonLibraryNet.LATEST\src\Apps\CommonLibrary.SampleApp\";

            // Settings for the Code model builders.
            ModelBuilderSettings settings = new ModelBuilderSettings()
            {
                ModelCodeLocation = baseLocCode + @"Generated\src",
                ModelInstallLocation = baseLocCode + @"Generated\install",
                ModelCodeLocationTemplate = baseLocComLib + @"src\lib\CommonLibrary.NET\CodeGen\Templates\Default",
                ModelDbStoredProcTemplates = baseLocComLib + @"src\lib\CommonLibrary.NET\CodeGen\Templates\DefaultSql",
                DbAction_Create = DbCreateType.Create,
                Connection = new ConnectionInfo("Server=kishore1;Database=testdb;User=testuser1;Password=password;", "System.Data.SqlClient"),
                AssemblyName = "CommonLibrary.Extensions"
            };
			
			//<doc:example>
            ModelContainer models = new ModelContainer()
            {
                Settings = settings,
                ExtendedSettings = new Dictionary<string, object>() { },
                
                // Model definition.
                AllModels = new List<Model>()
                {
                    new Model("ModelBase")
                            .AddProperty<int>( "Id").Required.Key
                            .AddProperty<DateTime>( "CreateDate").Required
                            .AddProperty<DateTime>( "UpdateDate").Required
                            .AddProperty<string>( "CreateUser").Required.MaxLength("20")
                            .AddProperty<string>( "UpdateUser").Required.MaxLength("20")
                            .AddProperty<string>( "UpdateComment").Required.MaxLength("150")
                            .AddProperty<bool>( "IsActive").Required.DefaultTo(1).Mod,

                    new Model("Address")
                            .AddProperty<string>("Street").Range("-1", "40")
                            .AddProperty<string>("City").Range("-1", "30")
                            .AddProperty<string>("State").Range("-1", "20")
                            .AddProperty<string>("Country").Range("-1", "20")
                            .AddProperty<string>("Zip").Range("-1", "10")
                            .AddProperty<int>("CityId")
                            .AddProperty<int>("StateId")
                            .AddProperty<int>("CountryId")
                            .AddProperty<bool>("IsOnline").Mod,                    

                    new Model("User")
                            .BuildCode().BuildTable("Users").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Account")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("UserName").Required.Range("3", "20")
                            .AddProperty<string>("UserNameLowered").Required.Range("3", "20")
                            .AddProperty<string>("Email").Required.Range("7", "30")
                            .AddProperty<string>("EmailLowered").Required.Range("7", "30")
                            .AddProperty<string>("Password").Required.Range("5", "100")
                            .AddProperty<string>("Roles").Range("-1", "50")
                            .AddProperty<string>("MobilePhone").Range("10", "20")
                            .AddProperty<string>("SecurityQuestion").Range("-1", "150")
                            .AddProperty<string>("SecurityAnswer").Range("-1", "150")
                            .AddProperty<string>("Comment").Range("-1", "50")
                            .AddProperty<bool>("IsApproved")
                            .AddProperty<bool>("IsLockedOut")
                            .AddProperty<string>("LockOutReason").Range("-1", "50")
                            .AddProperty<DateTime>("LastLoginDate").Required
                            .AddProperty<DateTime>("LastPasswordChangedDate").Required
                            .AddProperty<DateTime>("LastPasswordResetDate").Required
                            .AddProperty<DateTime>("LastLockOutDate").Required.Mod,

                    new Model("Comment")
                            .BuildCode().BuildTable("Comments").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Comments")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("RefId")
                            .AddProperty<string>("Title").Required.Range("1", "100")
                            .AddProperty<string>("Content").Required.Range("3", "250")
                            .AddProperty<int>("Rating").Mod,
                    
                    new Model("Event")
                            .BuildCode().BuildTable("Events").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Events")
                            .InheritsFrom("ModelBase")
                            .HasOne("User").OnKey("UserId")
                            .HasMany("Comment").OnForeignKey("RefId")
                            .HasComposition("Address")
                            .AddProperty<int>("UserId")
                            .AddProperty<string>("Title").Required.Range("2", "150")
                            .AddProperty<string>("Summary").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Description").Required.Range("2", "-1")
                            .AddProperty<DateTime>("StartDate").Required
                            .AddProperty<DateTime>("EndDate").Required
                            .AddProperty<int>("StartTime")
                            .AddProperty<int>("EndTime")
                            .AddProperty<string>("Email").Range("-1", "30").RegEx("RegexPatterns.Email")
                            .AddProperty<string>("Phone").Range("-1", "10").RegEx("RegexPatterns.PhoneUS")
                            .AddProperty<string>("Url").Range("-1", "150").RegEx("RegexPatterns.Url")
                            .AddProperty<string>("Keywords").Range("-1", "150").RegEx("RegexPatterns.Url")
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked")
                            .AddProperty<int>( "TotalBookMarked")
                            .AddProperty<int>( "TotalAbuseReports").Mod,
                }            
            };
			//</doc:example>
            return models;
        }        
    }


}
