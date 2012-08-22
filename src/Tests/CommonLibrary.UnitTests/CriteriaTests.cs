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


namespace CommonLibrary.Tests
{

    [TestFixture]
    public class CriteriaTests    
    {
        [Test]
        public void CanBuild1_Equals_ConditionWithString()
        {
            var criteria = Query<object>.New().From("users").Where("Name").Is("Kishore");
            var sql = criteria.Builder.Build();            
            var conditions = criteria.Builder.BuildConditions();
            var conditionsNoWhere = criteria.Builder.BuildConditions(false);

            Assert.AreEqual("select * from users where [Name] = 'Kishore'", sql.Trim());
            Assert.AreEqual("where [Name] = 'Kishore'", conditions.Trim());
            Assert.AreEqual("[Name] = 'Kishore'", conditionsNoWhere.Trim());
        }



        [Test]
        public void CanBuild1_LessThan_ConditionWithInt()
        {
            var criteria = Query<object>.New().From("users").Where("Id").LessThan(20);
            var sql = criteria.Builder.Build();

            Assert.AreEqual("select * from users where Id < 20", sql.Trim());
        }


        [Test]
        public void CanBuild1_In_ConditionWithInt()
        {
            var criteria = Query<object>.New().Where("Id").In<int>(3, 5, 7);
            var sql = criteria.Builder.BuildConditions();

            Assert.AreEqual("where Id in ( 3, 5, 7 )", sql.Trim());
        }


        [Test]
        public void CanBuild1_GreaterThan_ConditionWithDate()
        {
            var criteria = Query<object>.New().Where("CreateDate").MoreThan(DateTime.Today);
            var sql = criteria.Builder.BuildConditions(false);

            Assert.AreEqual("CreateDate > '" + DateTime.Today.ToShortDateString() + "'", sql.Trim());
        }


        [Test]
        public void CanBuildIsNull()
        {
            var criteria = Query<object>.New().Where("CreateDate").Null().And("CreateUser").Is("kishore");
            var sql = criteria.Builder.BuildConditions(false);

            Assert.AreEqual("CreateDate is null And CreateUser = 'kishore'", sql.Trim());
        }


        [Test]
        public void CanBuildIsNotNull()
        {
            var criteria = Query<object>.New().Where("CreateDate").NotNull().And("CreateUser").Is("kishore");
            var sql = criteria.Builder.BuildConditions(false);

            Assert.AreEqual("CreateDate is not null And CreateUser = 'kishore'", sql.Trim());
        }


        [Test]
        public void CanBuild2()
        {
            var criteria = Query<object>.New().Select("Title", "Desc")
                                         .From("users")
                                         .Where("Id").MoreThan(50)
                                         .Or("Name").NotIn<string>("kishore", "reddy")
                                         .OrderByDescending("Id")
                                         .OrderBy("Name")
                                         .End();
            var sql = criteria.Builder.Build();
            var select = criteria.Builder.BuildSelect();
            var selectNoSel = criteria.Builder.BuildSelect(false);
            var conditions = criteria.Builder.BuildConditions();
            var conditionsNoWhere = criteria.Builder.BuildConditions(false);
            var orderby = criteria.Builder.BuildOrderBy();
            var orderbyNoOrder = criteria.Builder.BuildOrderBy(false);

            Assert.AreEqual("select Title, Desc from users where Id > 50 Or [Name] not in ( 'kishore', 'reddy' )  order by Id Desc, [Name] Asc", sql.Trim());
            Assert.AreEqual("select Title, Desc", select.Trim());
            Assert.AreEqual("Title, Desc", selectNoSel.Trim());
            Assert.AreEqual("where Id > 50 Or [Name] not in ( 'kishore', 'reddy' )", conditions.Trim());
            Assert.AreEqual("Id > 50 Or [Name] not in ( 'kishore', 'reddy' )", conditionsNoWhere.Trim());
            Assert.AreEqual("order by Id Desc, [Name] Asc", orderby.Trim());
            Assert.AreEqual("Id Desc, [Name] Asc", orderbyNoOrder.Trim());
        }


        [Test]
        public void CanBuild2_WithOrderBy()
        {
            var criteria = Query<object>.New().From("users").Where("Id").MoreThan(50).Or("Name").NotIn<string>("kishore", "reddy").OrderByDescending("Id").OrderBy("Name").End();
            var sql = criteria.Builder.Build();

            Assert.AreEqual("select * from users where Id > 50 Or [Name] not in ( 'kishore', 'reddy' )  order by Id Desc, [Name] Asc", sql);
        }


              


        class Person
        {
            public string UserName { get; set; }
            public bool IsOver21 { get; set; }
        }
    }
}
