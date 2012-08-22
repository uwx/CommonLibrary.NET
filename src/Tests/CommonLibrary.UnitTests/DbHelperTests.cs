using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;


using NUnit.Framework;
using ComLib;
using ComLib.Data;


namespace CommonLibrary.Tests
{
    //[TestFixture]
    public class DbHelperTests
    {
        //[Test]
        public void CanGetTable()
        {
            var db = new Database(ConnectionInfo.Default);
            DataTable table = db.ExecuteDataTable("select * from wk_adminqueries", System.Data.CommandType.Text);

            Assert.IsNotNull(table);
        }


        //[Test]
        public void CanGetScalar()
        {
            var db = new Database(ConnectionInfo.Default);
            object totalRecords = db.ExecuteScalar("select count(*) from wk_adminqueries", System.Data.CommandType.Text);

            Assert.AreEqual(8, totalRecords);
        }


        //[Test]
        public void CanUpdateData()
        {
            var db = new Database(ConnectionInfo.Default);
            object maxId = db.ExecuteScalar("select max(id) from wk_adminqueries", System.Data.CommandType.Text);
            int rowsAffected = db.ExecuteNonQuery("update wk_adminqueries set description = 'unit test update' where id = " + maxId, CommandType.Text);

            Assert.AreEqual(1, rowsAffected);
        }


        //[Test]
        public void Can1()
        {
            var db = new Database(ConnectionInfo.Default);
            DbParameter[] dbparams = new DbParameter[2];
            dbparams[0] = db.BuildInParam("@groupid", DbType.Int32, 1);
            dbparams[1] = db.BuildInParam("@name", DbType.String, "aboutus");
            DataTable table = db.ExecuteDataTable("Kd_PageContent_Retrieve", CommandType.StoredProcedure, dbparams);
            string content = table.Rows[0]["Description"].ToString();
            Assert.IsNotNull(content);
         
        }
    }
}
