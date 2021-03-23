using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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
    public class UsersController : Controller
    {

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {

            //Selecting all users from DB and passing it to View
            var users = DbManager.GetUsers();
            return View(users);
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(string userId, string username)
        {

            if (userId.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentRole = DbManager.GetRoleByUserId(userId);

            var rolesSelectList = DbManager.GetRolesSelectList();

            var viewModel = new AssignRoleViewModel(currentRole, rolesSelectList, userId, username);

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(AssignRoleViewModel viewModel)
        {
            if (viewModel.CurrentRole != viewModel.NewRole)
            {
                DbManager.ChangeRoleOfUser(viewModel.UserId, viewModel.CurrentRole, viewModel.NewRole);
            }
            return RedirectToAction("Index");
        }




        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {

            if (id.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific user from DB
            var user = DbManager.GetUserById(id);

            var role = DbManager.GetRoleByUserId(id);

            //Passing role name to ViewBag.Role 
            ViewBag.Role = role;


            if (user.Equals(null))
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //Delete function is similar to Details
        //Passing same arguments to View
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {

            if (id.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific user from DB
            var user = DbManager.GetUserById(id);

            var role = DbManager.GetRoleByUserId(id);

            //Passing role name to ViewBag.Role 
            ViewBag.Role = role;

            if (user.Equals(null))
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(ApplicationUser user)
        {
            DbManager.DeleteUser(user);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Selecting specific user from DB
            var user = DbManager.GetUserById(id);

            if (user.Equals(null))
            {
                return HttpNotFound();
            }

            return View(user);
        }


        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(ApplicationUser user)
        {
            var u = DbManager.GetUserByUserName(user.UserName);

            if (u != null)
            {
                ModelState.AddModelError("UserName", "This user name allready exists!");
            }
            if (ModelState.IsValid)
            {

                DbManager.EditUser(user);
                return RedirectToAction("Index");

            }

            return View(user);

        }

        // GET: Users/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var rolesSelectList = DbManager.GetRolesSelectList();

            var viewModel = new UserViewModel(rolesSelectList);

            return View(viewModel);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(UserViewModel userViewModel)
        {
            var u = DbManager.GetUserByUserName(userViewModel.UserName);

            if (u != null)
            {
                ModelState.AddModelError("UserName", "This user name allready exists!");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() {
                    Email = userViewModel.Email, 
                    FirstName = userViewModel.FirstName,
                    SurName = userViewModel.SurName,
                    UserName = userViewModel.UserName
                };

                DbManager.AddUser(user, userViewModel.Password, userViewModel.RoleName);
                return RedirectToAction("Index");
                

            }
            var rolesSelectList = DbManager.GetRolesSelectList();
            userViewModel.RolesSelectList = rolesSelectList;
            return View(userViewModel);
        }


    }
}