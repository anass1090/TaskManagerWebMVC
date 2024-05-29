using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.DAL.Connection;
using TaskManager.DAL.FakeRepositories;
using TaskManager.Logic.Managers;
using TaskManager.Logic.Models;

namespace TaskManager.UnitTests.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        private FakeTaskRepository fakeTaskRepository;
        private TaskService taskService;

        [TestInitialize]
        public void Setup()
        {
            fakeTaskRepository = new();
            taskService = new(fakeTaskRepository);
        }

        [TestMethod]
        public void TestCreatingValidTask()
        {
            (Task task, string errorMessage) = taskService.CreateTask("Test Titel", "Test beschrijving", 3, 1);
            Assert.IsTrue(task != null && errorMessage == null);
        }

        [TestMethod]
        public void TestCreatingInValidTask()
        {
            (Task task, string errorMessage) = taskService.CreateTask(null, "Test beschrijving", 3, 1);
            Assert.IsTrue(task == null && errorMessage == "Not all required fields have been filled in, check this and try again.");
        }
    }
}
