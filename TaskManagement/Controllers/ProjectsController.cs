using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using TaskManagement.Models;
using TaskManagement.Utilities;
using TaskManagement.Buisness;
using TaskManagement.ViewModels;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {

        // GET: Projects
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Index()
        {

            //Returning list of Projects and passing it to View
            var projects = DbManager.GetProjects();

            return View(projects);
        }


        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult ListOfSuccessesWithId(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = DbManager.GetProjectById(id.Value);
            var progress = DbManager.GetProjectProgress(id.Value);
            var viewModel = new ProjectViewModel(project, progress);

            return View(viewModel);
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting project with specific Id
            var project = DbManager.GetProjectById(Id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create()
        {
            var rolesList = new List<String>();
            rolesList.Add("Project Manager");
            var usersSelectList = DbManager.GetUsersSelectList(rolesList);

            var viewModel = new ProjectViewModel(usersSelectList);

            return View(viewModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create(ProjectViewModel projectViewModel)
        {
            var project = DbManager.GetProjectByProjectCode(projectViewModel.Project.ProjectCode);
            
            if (project != null)
            {
                ModelState.AddModelError("Project.ProjectCode", "Project code allready exists!");
            }

            if (ModelState.IsValid)
            {
                //Getting values from text fields and adding them into DB
                DbManager.AddProject(projectViewModel.Project);
                return RedirectToAction("Index");

            }
            var rolesList = new List<String>();
            rolesList.Add("Project Manager");
            projectViewModel.UsersSelectList = DbManager.GetUsersSelectList(rolesList);
            return View(projectViewModel);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = DbManager.GetProjectById(id.Value);
            if (project == null)
            {
                return HttpNotFound();
            }
            var rolesList = new List<String>();
            rolesList.Add("Project Manager");
            var usersSelectList = DbManager.GetUsersSelectList(rolesList);

            var viewModel = new ProjectViewModel(project, usersSelectList);

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(ProjectViewModel projectViewModel)
        {
            var project = DbManager.GetProjectByProjectCode(projectViewModel.Project.ProjectCode);

            if (project != null)
            {
                ModelState.AddModelError("Project.ProjectCode", "Project code allready exists!");
            }
            if (ModelState.IsValid)
            {
                DbManager.EditProject(projectViewModel.Project);
                return RedirectToAction("Index");
            }

            var rolesList = new List<String>();
            rolesList.Add("Project Manager");
            projectViewModel.UsersSelectList = DbManager.GetUsersSelectList(rolesList);
            return View(projectViewModel);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific project from DB
            Project project = DbManager.GetProjectById(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Project project)
        {

            //Selecting specific project from DB and deleting it from DB
            DbManager.DeleteProject(project);

            return RedirectToAction("Index");
        }

    }
}