using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Taskmanager.DAL.Connection;


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
            Assert.IsNotNull(dataAccess.OpenConnection());
        }
    }
}
