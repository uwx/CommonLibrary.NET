using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Models;
using ComLib.Data;

namespace CommonLibrary.CodeGeneration
{
    public class SampleModels
    {
        private ConnectionInfo _conn;

        public SampleModels(ConnectionInfo conn)
        {
            _conn = conn;
        }
        

        public ModelContext GetModelContext()
        {
            return new ModelContext(GetModelContainer());
        }


        public static ModelContainer GetModelContainer()
        {
            string baseLoc = @"c:\dev\business\CommonLibrary.NET\CommonLibraryNet_LATEST\";
            // Settings for the Code model builders.
            ModelBuilderSettings settings = new ModelBuilderSettings()
            {
                ModelCodeLocation = baseLoc + @"src\Apps\CommonLibrary.CodeGeneration\Generated\src",
                ModelInstallLocation = baseLoc + @"src\Apps\CommonLibrary.CodeGeneration\Generated\install",
                ModelCodeLocationTemplate = baseLoc + @"src\lib\CommonLibrary.NET\CodeGen\Templates\Default",
                ModelDbStoredProcTemplates = baseLoc + @"src\lib\CommonLibrary.NET\CodeGen\Templates\DefaultSql",
                DbAction_Create = DbCreateType.Create,
                Connection = new ConnectionInfo("Server=kishore_pc1;Database=testdb2;User=testuser1;Password=password;", "System.Data.SqlClient"),
                AssemblyName = "CommonLibrary.Extensions"
            };

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
                    /*
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
                    
                    new Model("Blog")
                            .BuildCode().BuildTable("Blogs").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Blogs")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("Title").Required.Range("2", "150")
                            .AddProperty<string>("Summary").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Description").Required.Range("2", "-1")
                            .AddProperty<DateTime>("PublishDate").Required
                            .AddProperty<int>("Year").NoCode.GetterOnly
                            .AddProperty<int>("Month").NoCode.GetterOnly.Mod,
                    */
                    new Model("Event")
                            .BuildCode().BuildTable("Events").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Events")
                            .InheritsFrom("ModelBase")
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
                            .AddProperty<string>("Phone").Range("-1", "15").RegEx("RegexPatterns.PhoneUS")
                            .AddProperty<string>("Url").Range("-1", "150").RegEx("RegexPatterns.Url")
                            .AddProperty<string>("Keywords").Range("-1", "150").RegEx("RegexPatterns.Url")
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked")
                            .AddProperty<int>( "TotalBookMarked")
                            .AddProperty<int>( "TotalAbuseReports").Mod/*,

                    new Model("Product")
                            .BuildCode().BuildTable("Products").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Products")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("Make").Required.Range("2", "50")
                            .AddProperty<string>("Model").Required.Range("2", "50")                            
                            .AddProperty<DateTime>("AvailableDate")
                            .AddProperty<double>("Cost")
                            .AddProperty<bool>("IsInStock").Mod*/

                }
            }; 
            return models;
        }        
    }
}
