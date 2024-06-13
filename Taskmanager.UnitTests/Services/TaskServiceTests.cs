using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.DAL.FakeRepositories;
using TaskManager.Logic.Exceptions;
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
            Task task = taskService.CreateTask("Test Titel", "Test beschrijving", 3, 1);
            Assert.IsTrue(task.Title == "Test Titel" && task.Description == "Test beschrijving" && task.Project_Id == 3 && task.User_Id == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "Title and / or description is empty.")]
        public void TestCreatingInValidTaskByGivingNoTitle()
        {
            Assert.IsTrue(taskService.CreateTask(null, "Test beschrijving", 3, 1) == null);
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "Too many characters in your title or description, try again.")]
        public void TestCreatingInValidTaskByGivingTooLongTitle()
        {
            Assert.IsTrue(taskService.CreateTask("sdfgdsfgsdfgsdfgsdfgsdfffffffffffffffffffffdddddddddddddddddddddddddd", "Test beschrijving", 3, 1) == null);
        }
    }
}
