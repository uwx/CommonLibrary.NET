using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;

using ComLib;
using ComLib.Arguments;


namespace CommonLibrary.Tests
{
    // name, alias, desc, datatype, isrequired, example
    public class StartupArgs
    {
        [Arg("Config", "c", "config file for environment", typeof(string), true, "", "dev.xml")]
        public string Config { get; set; }


        [Arg("DateOffset", "d", "business date offset from today", typeof(int), true, 0, "0", "0, 2, -1")]
        public int BusinessDateOffset { get; set; }


        [Arg("BusDate", "b", "The business date", typeof(int), true, "${today}", "${today}", Interpret = true )]
        public DateTime BusinessDate { get; set; }


        [Arg("DryRun", "dr", "readonly mode", typeof(bool), false, false, "true", "true|false")]
        public bool DryRun { get; set; }

        
        [Arg(1, "Number of categories to display", typeof(int), true, 1, "1", "1|2|3 etc.")]
        public int CategoriesToDisplay { get; set; }


        [Arg(0, "settings id to load on startup", typeof(string), true, "settings_01", "settings_01")]
        public string DefaultSettingsId { get; set; }
    }



    [TestFixture]
    public class ArgsParserTests
    {
        public ArgsParserTests()
        {
            // Register the calls for parsing a string and doing substitutions.
            Args.InitServices((textargs) => ComLib.LexArgs.ParseList(textargs), (arg) => ComLib.Subs.Substitutor.Substitute(arg));
        }


        [Test]
        public void IsHelp()
        {
            Args args = new Args(new string[]{ "-help"});
            Assert.IsTrue(args.IsHelp);
        }


        [Test]
        public void IsVersion()
        {
            Args args = new Args(new string[] { "-version" });
            Assert.IsTrue(args.IsVersion);
        }


        [Test]
        public void IsPause()
        {
            Args args = new Args(new string[] { "-pause" });
            Assert.IsTrue(args.IsPause);
        }


        [Test]
        public void IsInfo()
        {
            Args args = new Args(new string[] { "-about" });
            Assert.IsTrue(args.IsInfo);
        }

        [Test]
        public void CanParseText()
        {            
            Args args = Args.Parse("-config:prod.xml -businessDate:T-1 myAppId").Item;
            //Args.Parse(
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["businessDate"], "T-1");
            Assert.AreEqual(args.Positional[0], "myAppId");
        }


        [Test]
        public void CanParseArgListWithNonDefaultNameValueIdentifiers()
        {
            string[] argList = new string[] { "@config=prod.xml", "@businessDate=T-1", "myApplicationId" };

            Args args = Args.Parse(argList, "@", "=", null).Item;

            Assert.AreEqual(args.Prefix, "@");
            Assert.AreEqual(args.Separator, "=");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["businessDate"], "T-1");
            Assert.AreEqual(args.Positional[0], "myApplicationId");
        }


        [Test]
        public void CanParseArgListWithSpaces()
        {
            string[] argList = new string[] { "@config=my prod.xml", "@businessDate=T-1", "myApplicationId" };

            Args args = Args.Parse(argList, "@", "=").Item;

            Assert.AreEqual(args.Prefix, "@");
            Assert.AreEqual(args.Separator, "=");
            Assert.AreEqual(args.Named["config"], "my prod.xml");
            Assert.AreEqual(args.Named["businessDate"], "T-1");
            Assert.AreEqual(args.Positional[0], "myApplicationId");
        }


        [Test]
        public void CanParseArgListWithOnlyKeysForBoolFlags()
        {
            string[] argList = new string[] { "-email", "-recurse", "-config:my prod.xml", "100" };

            Args args = Args.Parse(argList, "-", ":").Item;

            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "my prod.xml");
            Assert.AreEqual(args.Named["email"], "true");
            Assert.AreEqual(args.Named["recurse"], "true");
            Assert.AreEqual(args.Positional[0], "100");
        }


        [Test]
        public void CanParseTextWithNonDefaultNameValueIdentifiers()
        {
            Args args = Args.Parse("@config=prod.xml @businessDate=T-1 myApplicationId", "@", "=").Item;

            Assert.AreEqual(args.Prefix, "@");
            Assert.AreEqual(args.Separator, "=");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["businessDate"], "T-1");
            Assert.AreEqual(args.Positional[0], "myApplicationId");
        }


        [Test]
        public void CanParseTextWithQuotesWithNonDefaultNameValueIdentifiers()
        {
            Args args = Args.Parse("@config='prod.xml' @businessDate=T-1 'c:/program files/ccnet'", "@", "=").Item;

            Assert.AreEqual(args.Prefix, "@");
            Assert.AreEqual(args.Separator, "=");
            Assert.AreEqual(args.Named["config"], "'prod.xml'");
            Assert.AreEqual(args.Named["businessDate"], "T-1");
            Assert.AreEqual(args.Positional[0], "c:/program files/ccnet");
        }


        [Test]
        public void CanParseTextWithAttributes()
        {
            StartupArgs startupArgs = new StartupArgs();

            Args args = Args.Parse("-config:prod.xml -dateoffset:2 -busdate:${today} -dryrun:true", "-", ":", startupArgs).Item;

            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["dateoffset"], "2");
            Assert.AreEqual(args.Named["dryrun"], "true");
            Assert.AreEqual(startupArgs.Config, "prod.xml");
            Assert.AreEqual(startupArgs.BusinessDateOffset, 2);
            Assert.AreEqual(startupArgs.BusinessDate, DateTime.Today);
            Assert.AreEqual(startupArgs.DryRun, true);
        }


        [Test]
        public void CanParseTextWithAttributesWithCaseInsensitive()
        {
            StartupArgs startupArgs = new StartupArgs();
            string[] rawArgs = new string[] { "-config:prod.xml", "-dateoffset:2", "-busdate:${today}", "-dryrun:true"};
            Args args = Args.Parse(rawArgs, "-", ":", startupArgs).Item;

            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["dateoffset"], "2");
            Assert.AreEqual(args.Named["dryrun"], "true");
            Assert.AreEqual(startupArgs.Config, "prod.xml");
            Assert.AreEqual(startupArgs.BusinessDateOffset, 2);
            Assert.AreEqual(startupArgs.BusinessDate, DateTime.Today);
            Assert.AreEqual(startupArgs.DryRun, true);
        }


        [Test]
        public void CanParseTextWithAttributesWithAlias()
        {
            StartupArgs startupArgs = new StartupArgs();
            string[] rawArgs = new string[] { "-c:prod.xml", "-d:2", "-b:${today}", "-dr:true" };
            Args args = Args.Parse(rawArgs, "-", ":", startupArgs).Item;

            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["c"], "prod.xml");
            Assert.AreEqual(args.Named["d"], "2");
            Assert.AreEqual(args.Named["dr"], "true");
            Assert.AreEqual(startupArgs.Config, "prod.xml");
            Assert.AreEqual(startupArgs.BusinessDateOffset, 2);
            Assert.AreEqual(startupArgs.BusinessDate, DateTime.Today);
            Assert.AreEqual(startupArgs.DryRun, true);
        }


        [Test]
        public void CanCatchErrorsViaAttributes()
        {
            StartupArgs startupArgs = new StartupArgs();

            BoolMessageItem<Args> result = Args.Parse("-busdate:abc -dryrun:test", "-", ":", startupArgs);
            Args args = result.Item;

            Assert.IsFalse(result.Success);
            Assert.IsNotEmpty(result.Message);           
        }


        [Test]
        public void CanParseUnNamedArgs()
        {
            StartupArgs startupArgs = new StartupArgs();

            BoolMessageItem<Args> result = Args.Parse("-config:Prod -dryrun:true ShowAll 8", "-", ":", startupArgs);
            Args args = result.Item;

            Assert.IsFalse(result.Success);
            Assert.IsNotEmpty(result.Message);
            Assert.AreEqual(startupArgs.Config, "Prod");
            Assert.AreEqual(startupArgs.DryRun, true);
            Assert.AreEqual(startupArgs.DefaultSettingsId, "ShowAll");
            Assert.AreEqual(startupArgs.CategoriesToDisplay, 8);
        }


        [Test]
        public void CanUseSchemaWithFluentAPI()
        {
            Args args = new Args("-", ":");
            args.Schema.AddNamed<string>("config").Alias("c").Required.CaseSensitive.DefaultsTo("dev.config").Examples("dev.xml", "dev.xml | qa.config").Describe("Config file for environment")
                       .AddNamed<int>("dateoffset").Alias("d").Optional.CaseInSensitive.DefaultsTo(0).Examples("0", "0 | 1").Describe("business date offset from today")
                       .AddNamed<DateTime>("busdate").Alias("b").Required.CaseSensitive.DefaultsTo(DateTime.Today).Examples("${today}", "${today} | ${t-1}").Interpret
                       .AddNamed<bool>("dryrun").Alias("dr").Optional.CaseInSensitive.DefaultsTo(false).Examples("true", "true | false").Describe("Run in test mode")
                       .AddPositional<int>(0).Required.DefaultsTo(5).Examples("5", "5 | 8").Describe("Number of categories to display");

            string[] commandLineArgs = new string[] { "-config:prod.xml", "-dateoffset:2", "-busdate:${today}", "-dryrun:true", "18" };
            BoolMessage result = args.DoParse(commandLineArgs);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["dateoffset"], "2");
            Assert.AreEqual(args.Named["dryrun"], "true");
            Assert.AreEqual(args.Get<DateTime>("busdate"), DateTime.Today);
        }


        [Test]
        public void CanUseSchemaWithoutFluentAPI()
        {
            Args args = BuildSampleArgs("-", ":");
            string[] commandLineArgs = new string[] { "-config:prod.xml", "-dateoffset:2", "-busdate:${today}", "-dryrun:true", "18" };
            BoolMessage result = args.DoParse(commandLineArgs);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "prod.xml");
            Assert.AreEqual(args.Named["dateoffset"], "2");
            Assert.AreEqual(args.Named["dryrun"], "true");
            Assert.AreEqual(args.Get<DateTime>("busdate"), DateTime.Today);
        }



        [Test]
        public void CanUseSchemaWithCaseSensitivity()
        {
            Args args = BuildSampleArgs("-", ":");
            string[] commandLineArgs = new string[] { "-Config:prod.xml", "-Dateoffset:2", "-Busdate:${today}", "-DryRun:true", "18" };
            BoolMessage result = args.DoParse(commandLineArgs);
            Assert.IsFalse(result.Success);
        }


        [Test]
        public void CanDoParse()
        {
            Args args = new Args("-", ":");
            string[] argList = new string[] { "-email", "-recurse", "-config:my prod.xml", "100" };

            BoolMessage result =  args.DoParse(argList);
            Assert.AreEqual(args.Prefix, "-");
            Assert.AreEqual(args.Separator, ":");
            Assert.AreEqual(args.Named["config"], "my prod.xml");
            Assert.AreEqual(args.Named["email"], "true");
            Assert.AreEqual(args.Named["recurse"], "true");
            Assert.AreEqual(args.Positional[0], "100");
            Assert.IsNotNull(args.Raw);
        }


        private Args BuildSampleArgs(string argPrefix, string argKeyValueSeparator)
        {
            Args args = new Args(argPrefix, argKeyValueSeparator);
            args.Schema.AddNamed<string>("config", "c", true, "dev.config", "config file for environment", "dev.xml", "dev.xml | qa.config", false, true, "common", false)
            .AddNamed<int>("dateoffset", "d", true, 0, "business date offset from today", "0", "0 | 1 | -1", false, true, "common", false)
            .AddNamed<DateTime>("busdate", "b", true, DateTime.Today, "business date offset from today", "0", "0 | 1 | -1", true, true, "common", false)
            .AddNamed<bool>("dryrun", "dr", true, false, "testing mode", "true", "true | false", false, true, "common", false)
            .AddPositional<int>(0, true, 5, "Number of categories to display", "5", " 5 | 8 etc.", false, true, "common", false);

            return args;
        }
    }
}
