using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Buisness;
using TaskManagement.Models;
using TaskManagement.ViewModels;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public TasksController()
        {

        }
        public TasksController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "Administrator, Project Manager, Developer")]
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var isAdministrator = UserManager.GetRoles(currentUserId).Contains("Administrator");
            var tasksList = new List<Tasks>();
            if (isAdministrator)
            {
                tasksList = DbManager.GetAllTasks();
            }
            else
            {
                tasksList = DbManager.GetTasksForUser(currentUserId);
            }
            return View(tasksList);
        }



        [Authorize(Roles = "Administrator")]
        public ActionResult AssignTask()
        {

            //List of all tasks and users is sent to View
            ViewBag.Id = new SelectList(DbManager.GetAllTasks(), "Id", "Description");
            ViewBag.UserId = new SelectList(UserManager.Users.ToList(), "Id", "UserName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignTask(Tasks tasks)
        {


            if (ModelState.IsValid)
            {

                DbManager.EditTask(tasks);
                return RedirectToAction("Index");

            }

            return View(tasks);
        }

        // GET: Tasks/Details/5
        [Authorize(Roles = "Administrator, Project Manager, Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific Tasks from DB
            Tasks task = DbManager.GetTaskById(id.Value);

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create()
        {
            var currentUserId = User.Identity.GetUserId();
            var rolesList = new List<string>();
            rolesList.Add("Developer");
            if (UserManager.GetRoles(currentUserId).Contains("Administrator"))
            {
                rolesList.Add("Project Manager");
            }

            var projectsList = DbManager.GetProjectsSelectList();
            var statusesList = DbManager.GetStatusSelectList();
            var usersList = DbManager.GetUsersSelectList(rolesList);

            var viewModel = new TaskViewModel(projectsList, statusesList, usersList);

            return View(viewModel);
        }


        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create(TaskViewModel taskViewModel)
        {
            //checking if user has more than 3 tasks
            var userId = taskViewModel.Task.UserId;
            var count = DbManager.GetNumberOfTasksOfUser(userId);
            if (count > 3)
            {
                ModelState.AddModelError("Task.UserId", "Select another assigne! This one has 3 tasks assigned!");
            }

            if (ModelState.IsValid)
            {
                //on task creating, we are setting task status: new
                taskViewModel.Task.StatusId = (int) Statuses.New;
                DbManager.AddTask(taskViewModel.Task);
                return RedirectToAction("Index");

            }


            var currentUserId = User.Identity.GetUserId();
            var rolesList = new List<string>();
            rolesList.Add("Developer");
            if (UserManager.GetRoles(currentUserId).Contains("Administrator"))
            {
                rolesList.Add("Project Manager");
            }

            taskViewModel.ProjectsSelectList = DbManager.GetProjectsSelectList();
            taskViewModel.StatusesSelectList = DbManager.GetStatusSelectList();
            taskViewModel.UsersSelectList = DbManager.GetUsersSelectList(rolesList);
            return View(taskViewModel);
        }

        // GET: Tasks/Edit/5
        [Authorize(Roles = "Administrator, Project Manager, Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentUserId = User.Identity.GetUserId();
            var rolesList = new List<string>();
            rolesList.Add("Developer");
            if (UserManager.GetRoles(currentUserId).Contains("Administrator"))
            {
                rolesList.Add("Project Manager");
            }

            var task = DbManager.GetTaskById(id.Value);
            var projectsList = DbManager.GetProjectsSelectList();
            var statusesList = DbManager.GetStatusSelectList();
            var usersList = DbManager.GetUsersSelectList(rolesList);

            var viewModel = new TaskViewModel(task, projectsList, statusesList, usersList);

            return View(viewModel);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Project Manager, Developer")]
        public ActionResult Edit(TaskViewModel taskViewModel)
        {
            //checking if user has more than 3 tasks
            var userId = taskViewModel.Task.UserId;
            var count = DbManager.GetNumberOfTasksOfUser(userId);
            if (count > 3)
            {
                ModelState.AddModelError("Task.UserId", "Select another assigne! This one has 3 tasks assigned!");
            }

            if (ModelState.IsValid)
            {
                DbManager.EditTask(taskViewModel.Task);
                return RedirectToAction("Index");
            }

            var currentUserId = User.Identity.GetUserId();
            var rolesList = new List<string>();
            rolesList.Add("Developer");
            if (UserManager.GetRoles(currentUserId).Contains("Administrator"))
            {
                rolesList.Add("Project Manager");
            }

            taskViewModel.ProjectsSelectList = DbManager.GetProjectsSelectList();
            taskViewModel.StatusesSelectList = DbManager.GetStatusSelectList();
            taskViewModel.UsersSelectList = DbManager.GetUsersSelectList(rolesList);
            return View(taskViewModel);
        }

        // GET: Tasks/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific task from DB
            Tasks task = DbManager.GetTaskById(id.Value);

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Tasks task)
        {
            //Selecting specific task and removing it from DB
            DbManager.DeleteTask(task);
            return RedirectToAction("Index");
        }




    }
}