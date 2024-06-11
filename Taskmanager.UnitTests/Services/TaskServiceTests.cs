using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.DAL.Connection;
using TaskManager.DAL.FakeRepositories;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Managers;
using TaskManager.Logic.Models;

namespace TaskManager.UnitTests.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        private ITaskRepository fakeTaskRepository;
        private TaskService taskService;

        [TestInitialize]
        public void Setup()
        {
            fakeTaskRepository = new FakeTaskRepository();
            taskService = new(fakeTaskRepository);
        }

        [TestMethod]
        public void TestCreatingValidTask()
        {
            // check how to check exceptions in 
            (Task task, string errorMessage) = taskService.CreateTask("Test Titel", "Test beschrijving", 3, 1);
            Assert.IsTrue(task != null && errorMessage == null);
        }

        [TestMethod]
        public void TestCreatingInValidTaskByGivingNoTitle()
        {
            (Task task, string errorMessage) = taskService.CreateTask(null, "Test beschrijving", 3, 1);
            Assert.IsTrue(task == null && errorMessage == "Not all required fields have been filled in, check this and try again.");
        }

        [TestMethod]
        public void TestCreatingInValidTaskByGivingTooLongTitle()
        {
            (Task task, string errorMessage) = taskService.CreateTask("sdfgdsfgsdfgsdfgsdfgsdfffffffffffffffffffffdddddddddddddddddddddddddd", "Test beschrijving", 3, 1);
            Assert.IsTrue(task == null && errorMessage == "Too many characters in your title or description, try again.");
        }
    }
}
