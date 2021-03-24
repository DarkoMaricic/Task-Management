using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TaskManagement.Models;
using TaskManagement.Controllers;
using System.Web.Mvc;
using TaskManagement.ViewModels;

namespace TaskManagement.Tests.Controllers
{

    [TestClass]
    public class TasksControllerTest {

        public ApplicationDbContext db = new ApplicationDbContext();
        

        [TestMethod]
        public void Create() {

            //Creating new object of type Tasks
            Tasks task = new Tasks();

            //Assigning values to our object
            DateTime dt = DateTime.Today;
            task.ProjectId = 1;
            task.StatusId = 1;
            task.UserId = "434c0705-184e-4ef0-b7c7-77e519c44b44";

            task.Progress = 0;
            task.Deadline = dt;
            task.Description = "Test description for new task";

            TaskViewModel taskViewModel = new TaskViewModel()
            {
                Task = task
            };


            //Creating instance of TasksController
            var tasksController = new TasksController();

            //Creating object of type RedirectToRouteResult because after calling post method CREATE, it returns RedirectToAction
            RedirectToRouteResult result = tasksController.Create(taskViewModel) as RedirectToRouteResult;

            //Checking if Create method redirects to Index() page after execution
            Assert.AreEqual("Index", result.RouteValues["Action"]);

            //Checking if result value is not null
            Assert.IsNotNull(result.ToString());

        }




        [TestMethod]
        public void Details()
        {

            //Selecting Tasks from DB with Id = 1
            var taskId = db.Tasks.First().Id;

            //Creating instance of TasksController
            var tasksController = new TasksController();

            //Calling Details() function from TasksController and casted it to ViewResult
            ViewResult result = tasksController.Details(taskId) as ViewResult;

            //Casting View model to actual object type in order to access its attributes for comparison
            var pr = (Tasks)result.ViewData.Model;

            //Checking if instance type from View is equal to type Tasks
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Tasks));

            //Checking if task Id we got from our View is equal to our selected Task from DB
            Assert.AreEqual(taskId, pr.Id);

        }

    }
}
