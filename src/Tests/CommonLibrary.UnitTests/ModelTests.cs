using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;

using ComLib;
using ComLib.Data;
using ComLib.Models;


namespace CommonLibrary.Tests
{

    [TestFixture]
    public class ModelTests    
    {
        public ModelContext GetContext()
        {
            string location = "";
            string comlib = "";
            var _conn = new ConnectionInfo("");

            // Settings for the Code model builders.
            ModelBuilderSettings settings = new ModelBuilderSettings()
            {               
                ModelCodeLocation = location + @"\src\lib\CommonLibraryNet.Web.Modules\Src\_Models\",
                ModelInstallLocation = location + @"\Install\",
                ModelCodeLocationTemplate = comlib + @"\src\Lib\CommonLibrary.NET\CodeGen\Templates\Default",
                ModelDbStoredProcTemplates = comlib + @"\src\Lib\CommonLibrary.NET\CodeGen\Templates\DefaultSql",
                DbAction_Create = DbCreateType.DropCreate,
                Connection = _conn,
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
                            .AddProperty<string>( "UpdateComment").Required.MaxLength("150"),

                    new Model("Rating")                            
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked")
                            .AddProperty<int>( "TotalBookMarked")
                            .AddProperty<int>( "TotalAbuseReports"),

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

                    new Model("Page")
                            .BuildCode().BuildTable("Pages").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Pages")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "100")
                            .AddProperty<string>("Description").Range("-1", "100")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<int>("IsPublished")
                            .AddProperty<string>("Keywords").Required.Range("-1", "80")
                            .AddProperty<string>("Slug").Range("-1", "150")
                            .AddProperty<string>("AccessRoles").Range("-1", "50")
                            .AddProperty<int>("Parent")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<bool>("IsFrontPage").Mod,

                    new Model("Part")
                            .BuildCode().BuildTable("Parts").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Parts")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "100")
                            .AddProperty<string>("Description").Range("-1", "100")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<string>("AccessRoles").Range("-1", "50")
                            .AddProperty<bool>("IsPublic").Mod,

                   new Model("Post")
                            .BuildCode().BuildTable("Posts").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Posts")
                            .InheritsFrom("ModelBase")                           
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<string>("Description").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<DateTime>("PublishDate")
                            .AddProperty<bool>("IsPublished")                            
                            .AddProperty<string>("Author").NoCode.NotPersisted
                            .AddProperty<string>("Tags").Range("-1", "80")                          
                            .AddProperty<string>("Slug").Range("-1", "150")
                            .AddProperty<bool>("IsFavorite")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<bool>("IsCommentEnabled")
                            .AddProperty<bool>("IsCommentModerated")
                            .AddProperty<bool>("IsRatable")
                            .AddProperty<int>("Year").GetterOnly.NoCode
                            .AddProperty<int>("Month").GetterOnly.NoCode
                            .AddProperty<int>("CommentCount")
                            .AddProperty<int>("ViewCount")
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked").Mod,

                    new Model("Event")
                            .BuildCode().BuildTable("Events").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.WebModules.Events")
                            .InheritsFrom("ModelBase")
                            .HasComposition("Address")
                            .HasInclude("Rating")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<string>("Description").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<DateTime>("PublishDate").NoCode.NotPersisted
                            .AddProperty<bool>("IsPublished")
                            .AddProperty<DateTime>("StartDate").Required
                            .AddProperty<DateTime>("EndDate").Required
                            .AddProperty<int>("StartTime")
                            .AddProperty<int>("EndTime")
                            .AddProperty<string>("Email").Range("-1", "30").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Phone").Range("-1", "14").RegExConst("RegexPatterns.PhoneUS")
                            .AddProperty<string>("Url").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<string>("Tags").Range("-1", "80").Mod
                }
            };
            return new ModelContext() { AllModels = models };
        }



        [Test]
        public void CanGet()
        {
            var ctx = GetContext();

            Assert.AreEqual(ctx.AllModels.Count, 7);
            Assert.AreEqual(ctx.AllModels["Post"].Name, "Post");
            Assert.AreEqual(ctx.AllModels[5].Name, "Post");
            Assert.AreEqual(ctx.AllModels.InheritanceFor("Post").Count, 2);
            Assert.AreEqual(ctx.AllModels.InheritanceFor("Post")[0].Name, "ModelBase");
            Assert.AreEqual(ctx.AllModels.InheritanceFor("Post")[1].Name, "Post");
        }


        [Test]
        public void CanAddRemove()
        {
            var ctx = GetContext();
            ctx.AllModels.Remove("Post");
            ctx.AllModels.Add(new Model() { Name = "CustomModel" });

            Assert.IsFalse(ctx.AllModels.Contains("Post"));
            Assert.IsTrue(ctx.AllModels.Contains("CustomModel"));
        }


        [Test]
        public void CanIterate()
        {
            var ctx = GetContext();
            int codeableModelCount = 0;
            
            ctx.AllModels.Iterate(m => m.GenerateCode, m => codeableModelCount++);

            Assert.AreEqual(codeableModelCount, 4);
        }


        [Test]
        public void CanIterateDetailed()
        {
            var ctx = GetContext();
            int countInheritedModels = 0;
            int countIncludes = 0;
            int countCompositions = 0;

            ctx.AllModels.Iterate("Event", 
                    (m)          => countInheritedModels++,
                    (m, include) => countIncludes++,
                    (m, compose) => countCompositions++,
                     null, null);

            Assert.AreEqual(countInheritedModels, 2);
            Assert.AreEqual(countIncludes, 1);
            Assert.AreEqual(countCompositions, 1);
        }


        [Test]
        public void CanIterateDetailed2()
        {
            var ctx = GetContext();
            string currentModelName = "";

            Dictionary<string, int> map = new Dictionary<string, int>();
            map["Post.CountInheritedModels"] = 0;
            map["Post.CountIncludes"] = 0;
            map["Post.CountCompositions"] = 0;

            map["Event.CountInheritedModels"] = 0;
            map["Event.CountIncludes"] = 0;
            map["Event.CountCompositions"] = 0;
            
            map["Page.CountInheritedModels"] = 0;
            map["Page.CountIncludes"] = 0;
            map["Page.CountCompositions"] = 0;
            
            map["Part.CountInheritedModels"] = 0;
            map["Part.CountIncludes"] = 0;
            map["Part.CountCompositions"] = 0;
            
            ctx.AllModels.Iterate( m => m.GenerateCode, 
                    (m)          => currentModelName = m.Name,
                    (m)          => map[currentModelName + ".CountInheritedModels"] = map[currentModelName + ".CountInheritedModels"]+ 1,
                    (m, include) => map[currentModelName + ".CountIncludes"] = map[currentModelName + ".CountIncludes"] + 1,
                    (m, compose) => map[currentModelName + ".CountCompositions"] = map[currentModelName + ".CountCompositions"]+ 1,
                    null, null);

            Assert.AreEqual(map["Post.CountInheritedModels"], 2);
            Assert.AreEqual(map["Post.CountIncludes"], 0);
            Assert.AreEqual(map["Post.CountCompositions"], 0);

            Assert.AreEqual(map["Event.CountInheritedModels"], 2);
            Assert.AreEqual(map["Event.CountIncludes"], 1);
            Assert.AreEqual(map["Event.CountCompositions"], 1);
        }
    }
}
