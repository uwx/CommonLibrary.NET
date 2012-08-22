using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;

using ComLib;
using ComLib.Environments;


namespace CommonLibrary.Tests
{


    [TestFixture]
    public class EnvrionmentTests
    {
        /// <summary>
        /// This builds a datastructure of all the environments supported
        /// and the links to the config files for each environment 
        /// and how they are inherited.
        /// 
        /// THIS CAN BE LOADED FROM AN XML, JSON, YAML, INI file or whatever.
        /// </summary>
        /// <returns></returns>
        public static List<EnvItem> GetSampleEvironments()
        {
            List<EnvItem> envs = new List<EnvItem>()
            {
                new EnvItem(){ Name = "Dev",    RefPath ="dev.config",    InheritsDeeply = true,  EnvType = EnvType.Dev,       Inherits = "" },
                new EnvItem(){ Name = "Dev2",   RefPath ="dev2.config",   InheritsDeeply = true,  EnvType = EnvType.Dev,       Inherits = "" },
                new EnvItem(){ Name = "Qa",     RefPath ="qa.config",     InheritsDeeply = true,  EnvType = EnvType.Qa,        Inherits = "Dev" },
                new EnvItem(){ Name = "Prod",   RefPath ="prod.config",   InheritsDeeply = true,  EnvType = EnvType.Prod,      Inherits = "Qa" },
                new EnvItem(){ Name = "MyProd", RefPath ="custom.config", InheritsDeeply = true,  EnvType = EnvType.MixedProd, Inherits = "Prod,Dev2" }
            };
            return envs;
        }


        [Test]
        public void CanSetEnvViaNameOnly()
        {
            // 1. Setup current env using default environment names/types.
            Envs.Set("prod");
            
            Assert.AreEqual(Env.Name, "prod");
            Assert.AreEqual(Env.EnvType, EnvType.Prod);
            Assert.AreEqual(Env.IsDev, false);
            Assert.AreEqual(Env.IsProd, true);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);
            Assert.AreEqual(Env.Available[0], "prod");
            Assert.AreEqual(Env.Available[1], "uat");
            Assert.AreEqual(Env.Available[2], "qa");
            Assert.AreEqual(Env.Available[3], "dev");
            Assert.AreEqual(Env.Selected, Env.Get("prod"));
            Assert.AreEqual(Env.Count, 4);
        }


        [Test]
        public void CanSetEnvViaNameAndType()
        {
            // 1. Setup current env using supplied environment names/types.
            // Not that <envName>:<envType> is the format
            // where <envType> = prod | uat | qa | dev
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,uat,qa,dev");

            Assert.AreEqual(Env.Name, "ny.prod");
            Assert.AreEqual(Env.EnvType, EnvType.Prod);
            Assert.AreEqual(Env.IsDev, false);
            Assert.AreEqual(Env.IsProd, true);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);
            Assert.AreEqual(Env.Available[0], "ny.prod");
            Assert.AreEqual(Env.Available[1], "london.prod");
            Assert.AreEqual(Env.Available[2], "uat");
            Assert.AreEqual(Env.Available[3], "qa");
            Assert.AreEqual(Env.Available[4], "dev");
            Assert.AreEqual(Env.Selected, Env.Get("ny.prod"));
            Assert.AreEqual(Env.Count, 5);
        }


        [Test]
        public void CanSetEnvViaBuildEnvs()
        {
            // 1. Setup current env using supplied environment names/types.
            // Not that <envName>:<envType> is the format
            // where <envType> = prod | uat | qa | dev
            List<EnvItem> available = GetSampleEvironments();
            Envs.Set("Dev2", available);

            Assert.AreEqual(Env.Name, "Dev2");
            Assert.AreEqual(Env.EnvType, EnvType.Dev);
            Assert.AreEqual(Env.IsDev, true);
            Assert.AreEqual(Env.IsProd, false);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);
            Assert.AreEqual(Env.Available[0], "Dev");
            Assert.AreEqual(Env.Available[1], "Dev2");
            Assert.AreEqual(Env.Available[2], "Qa");
            Assert.AreEqual(Env.Available[3], "Prod");
            Assert.AreEqual(Env.Available[4], "MyProd");
            Assert.AreEqual(Env.Selected, Env.Get("Dev2"));
            Assert.AreEqual(Env.Count, 5);
        }


        [Test]
        public void CanSetEnvAndConfigInheritance()
        {
            // 1. Setup current env using supplied environment names/types. 
            //    and all the available environments.
            // 2. Also supply the refpaths( config paths in this case ).
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,uat,qa,dev", "prod.config,qa.config,dev.config", false, false);
            
            Assert.AreEqual(Env.Name, "ny.prod");
            Assert.AreEqual(Env.EnvType, EnvType.Prod);
            Assert.AreEqual(Env.IsDev, false);
            Assert.AreEqual(Env.IsProd, true);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);
            Assert.AreEqual(Env.RefPath, "prod.config,qa.config,dev.config");
            Assert.AreEqual(Env.Available[0], "ny.prod");
            Assert.AreEqual(Env.Available[1], "london.prod");
            Assert.AreEqual(Env.Available[2], "uat");
            Assert.AreEqual(Env.Available[3], "qa");
            Assert.AreEqual(Env.Available[4], "dev");
            Assert.AreEqual(Env.Selected, Env.Get("ny.prod"));
            Assert.AreEqual(Env.Count, 5);
        }


        [Test]
        public void CanSetEnvAndConfigInheritance2()
        {
            // 1. Setup current env using supplied environment names/types. 
            //    and all the available environments.
            // 2. Also supply the refpaths( config paths in this case ).
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,uat,qa,dev", "ny.prod.config,london.prod.config,uat.config,qa.config,dev.config", true, false);

            Assert.AreEqual(Env.Name, "ny.prod");
            Assert.AreEqual(Env.EnvType, EnvType.Prod);
            Assert.AreEqual(Env.IsDev, false);
            Assert.AreEqual(Env.IsProd, true);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);            
            Assert.AreEqual(Env.Available[0], "ny.prod");
            Assert.AreEqual(Env.Available[1], "london.prod");
            Assert.AreEqual(Env.Available[2], "uat");
            Assert.AreEqual(Env.Available[3], "qa");
            Assert.AreEqual(Env.Available[4], "dev");
            Assert.AreEqual(Env.Selected, Env.Get("ny.prod"));
            
            // Check that the refpaths ( config files) have been set on the respective envs)
            Assert.AreEqual(Env.Get("ny.prod").RefPath, "ny.prod.config");
            Assert.AreEqual(Env.Get("qa").RefPath, "qa.config");
            Assert.AreEqual(Env.Get("dev").RefPath, "dev.config");

            Assert.AreEqual(Env.Count, 5);
        }


        [Test]
        public void CanChangeEnv()
        {
            // 1. Setup current env using supplied environment names/types. 
            //    and all the available environments.
            // 2. Also supply the refpaths( config paths in this case ).
            Envs.Set("prod", "prod,uat,qa,dev", "prod.config,uat.config,qa.config,dev.config", true, false);

            Assert.AreEqual(Env.Name, "prod");
            Assert.AreEqual(Env.EnvType, EnvType.Prod);
            Assert.AreEqual(Env.IsDev, false);
            Assert.AreEqual(Env.IsProd, true);
            Assert.AreEqual(Env.IsQa, false);
            Assert.AreEqual(Env.IsUat, false);
            Assert.AreEqual(Env.Available[0], "prod");
            Assert.AreEqual(Env.Available[1], "uat");
            Assert.AreEqual(Env.Available[2], "qa");
            Assert.AreEqual(Env.Available[3], "dev");
            Assert.AreEqual(Env.Selected, Env.Get("prod"));

            // Check that the refpaths ( config files) have been set on the respective envs)
            Assert.AreEqual(Env.Get("prod").RefPath, "prod.config");
            Assert.AreEqual(Env.Get("qa").RefPath, "qa.config");
            Assert.AreEqual(Env.Get("dev").RefPath, "dev.config");

            // Now change environment.
            Env.OnChange += ((sender, eventArgs) =>
            {
                Assert.AreEqual(Env.Name, "qa");
                Assert.AreEqual(Env.EnvType, EnvType.Qa);
                Assert.AreEqual(Env.IsDev, false);
                Assert.AreEqual(Env.IsProd, false);
                Assert.AreEqual(Env.IsQa, true);
                Assert.AreEqual(Env.IsUat, false);
                Assert.AreEqual(Env.Available[0], "prod");
                Assert.AreEqual(Env.Available[1], "uat");
                Assert.AreEqual(Env.Available[2], "qa");
                Assert.AreEqual(Env.Available[3], "dev");
                Assert.AreEqual(Env.Selected, Env.Get("qa"));

                // Check that the refpaths ( config files) have been set on the respective envs)
                Assert.AreEqual(Env.Get("prod").RefPath, "prod.config");
                Assert.AreEqual(Env.Get("qa").RefPath, "qa.config");
                Assert.AreEqual(Env.Get("dev").RefPath, "dev.config");
            });

            Env.Change("qa");
            Assert.AreEqual(Env.Count, 4);
        }


        [Test]
        public void CanSetProdConfig()
        {
            // 2. Non-Strict setup indicating current environment and config references.
            Envs.Set("prod", "prod,uat,qa,dev", "prod.config");
            
            // 3. Non-Strict setup indicating current environment and config inheritance 2 files.
            Envs.Set("dev", "prod,uat,qa,dev", "dev1.config,dev2.config");
            
            // 4. Non-Strict setup indicating current environment inheritance and config inheritance.
            Envs.Set("prod", "prod,uat,qa,dev", "prod.config,qa.config,dev.config");
            //EnvironmentCurrent.Change("qa");
            
            // 5. Strict setup indicating current environments selected from the possible environments.
            //Envs.Set(GetSampleEvironments(), "Prod");
        }
    }
}
