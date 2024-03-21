using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskManager.DAL.Connection;


namespace TaskManager.UnitTests.DAL
{
    [TestClass]
    public class DataAccessTests
    {
        private DataAccess dataAccess;

        [TestInitialize]
        public void Setup()
        {
            dataAccess = new DataAccess();
        }

        [TestMethod]
        public void TestDatabaseConnection()
        {
            dataAccess.OpenConnection();

            Assert.IsTrue(dataAccess.Connection.State == System.Data.ConnectionState.Open);
        }
    }
}
