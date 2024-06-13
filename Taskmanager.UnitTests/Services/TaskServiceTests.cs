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

        // CREATE TESTS
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

        [TestMethod]
        [ExpectedException(typeof(DatabaseException), "Something went wrong, contact customer support or try again later.")]
        public void TestCreatingInValidTaskByDatabaseError()
        {
            Assert.IsTrue(taskService.CreateTask("Test titel", "Test beschrijving", 3, 999) == null);
        }

        // READ TESTS
        [TestMethod]
        public void TestGettingValidTask()
        {
            int validTaskId = 1;
            int validUserId = 1;
            Task task = taskService.GetTaskById(validTaskId, validUserId);
            Assert.IsNotNull(task);
            Assert.AreEqual("Test title", task.Title);
            Assert.AreEqual("Test description", task.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "Task does not exist.")]
        public void TestGettingInvalidTaskById()
        {
            int invalidTaskId = -1;
            int validUserId = 1;
            Assert.IsNull(taskService.GetTaskById(invalidTaskId, validUserId));
        }

        [TestMethod]
        [ExpectedException(typeof(UserException), "User is not logged in.")]
        public void TestGettingTaskWithInvalidUserId()
        {
            int validTaskId = 1;
            int? invalidUserId = null;
            Assert.IsNull(taskService.GetTaskById(validTaskId, invalidUserId));
        }

        [TestMethod]
        [ExpectedException(typeof(UserException), "User does not have access to this task.")]
        public void TestGettingTaskWithUnauthorizedUser()
        {
            int validTaskId = 1;
            int unauthorizedUserId = 2;
            Assert.IsNull(taskService.GetTaskById(validTaskId, unauthorizedUserId));
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException), "Something went wrong, contact customer support.")]
        public void TestGettingTaskWithDatabaseError()
        {
            int taskIdWithDatabaseError = 999;
            int validUserId = 1;
            Assert.IsNull(taskService.GetTaskById(taskIdWithDatabaseError, validUserId));
        }
    }
}
