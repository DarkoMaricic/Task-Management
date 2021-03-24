using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Models;
using TaskManagement.Controllers;
using System.Web.Mvc;
using TaskManagement.ViewModels;

namespace TaskManagement.Tests.Controllers
{

    [TestClass]
    public class ProjectsControllerTest {

        public ApplicationDbContext db = new ApplicationDbContext();

        [TestMethod]
        public void Index() {

            //Creating instance of ProjectController
            var projectController = new ProjectsController();

            //Calling Index() function from ProjectController and casted it to ViewResult (returns View())
            var result = projectController.Index() as ViewResult;

            //Creating list of projects and assigning our result model type to list
            List<Project> myProjects = (List<Project>) result.Model;
  
            //Checking if our list object is not-null.
            Assert.IsNotNull(myProjects);
    
        }


        [TestMethod]
        public void Create()
        {

            //Creating new object of type Project
            Project project = new Project();

            //Assigning values to our object
            project.ProjectCode = DateTime.Now.ToString();
            project.ProjectName = "Test Name";

            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.Project = project;

            //Creating instance of ProjectController
            var projectController = new ProjectsController();

            //Creating object of type RedirectToRouteResult because after calling post method CREATE, it returns RedirectToAction
            RedirectToRouteResult result = projectController.Create(projectViewModel) as RedirectToRouteResult;

            //Checking if Create method redirects to Index() page after execution
            Assert.AreEqual("Index", result.RouteValues["Action"]);

            //Checking if result value is not null
            Assert.IsNotNull(result.ToString());
        }


        [TestMethod]
        public void Details() {

            //Selecting Project from DB with Id
            var projectId = db.Projects.First().Id;

            //Creating instance of ProjectController
            var projectController = new ProjectsController();

            //Calling Details() function from ProjectController and casted it to ViewResult
            ViewResult result = projectController.Details(projectId) as ViewResult;

            //Casting View model to actual object type in order to access its attributes for comparison
            var pr = (Project) result.ViewData.Model;

            //Checking if instance type from View is equal to type Project
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Project));

            //Checking if project Id we got from our View is equal to our selected Project from DB
            Assert.AreEqual(projectId, pr.Id);

        }


    }
}
