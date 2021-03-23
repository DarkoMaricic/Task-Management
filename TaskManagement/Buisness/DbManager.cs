using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskManagement.Models;

namespace TaskManagement.Buisness
{
    public class DbManager
    {
        public static List<Project> GetProjects()
        {
            using (var context = new ApplicationDbContext())
            {
                var projects = context.Projects.Include(p => p.User).ToList();
                //foreach (var project in projects) {
                //    var user = context.Users.FirstOrDefault(u => u.Id == project.UserId);
                //    project.User = user;
                //} 
                return projects;
            }
        }

        public static Project GetProjectById(int projectId)
        {
            using (var context = new ApplicationDbContext())
            {
                var project = context.Projects.Include(p => p.User).FirstOrDefault(p => p.Id == projectId);
                return project;
            }
        }


        public static Project GetProjectByProjectCode(string projectCode)
        {
            using (var context = new ApplicationDbContext())
            {
                var project = context.Projects.FirstOrDefault(p => p.ProjectCode == projectCode);
                return project;
            }
        }

       
        public static void AddProject(Project project)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }
        }


        public static void DeleteProject(Project project)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(project).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public static void EditProject(Project project)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
        }



        public static List<SelectListItem> GetUsersSelectList(List<String> rolesNameList)
        {
            var result = new List<SelectListItem>();
            using (var context = new ApplicationDbContext())
            {

                var roles = context.Roles.Where(r => rolesNameList.Contains(r.Name)).ToList();
                var rolesIdList = roles.Select(r => r.Id).ToList();
                var users = context.Users.Where(u => u.Roles.Any(r => rolesIdList.Contains(r.RoleId))).ToList();

                foreach (var user in users)
                {
                    var item = new SelectListItem();
                    item.Value = user.Id;
                    item.Text = user.FirstName + " " + user.SurName;
                    result.Add(item);
                }

            }

            return result;
        }
        public static float GetProjectProgress(int projectId)
        {
            using (var context = new ApplicationDbContext())
            {
                var project = context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
                var listTasks = project.Tasks;
                float sum = 0;
                var counter = 0;
                foreach (var task in listTasks)
                {
                    sum = sum + task.Progress;
                    counter += 1;
                }
                if (counter != 0)
                {
                    float result = sum / counter;
                    return result;
                }
                else
                {
                    return 0;
                }


            }
        }
        public static List<SelectListItem> GetProjectsSelectList()
        {
            var result = new List<SelectListItem>();
            using (var context = new ApplicationDbContext())
            {

                var projects = context.Projects.ToList();

                foreach (var project in projects)
                {
                    var item = new SelectListItem();
                    item.Value = project.Id.ToString();
                    item.Text = project.ProjectName + " (" + project.ProjectCode + ")";
                    result.Add(item);
                }

            }

            return result;
        }
        public static List<SelectListItem> GetStatusSelectList()
        {
            var result = new List<SelectListItem>();
            using (var context = new ApplicationDbContext())
            {

                var statuses = context.Status.ToList();
            

                foreach (var status in statuses)
                {
                    var item = new SelectListItem();
                    item.Value = status.Id.ToString();
                    item.Text = status.Name;
                    result.Add(item);
                }

            }

            return result;
        }



        public static void AddTask(Tasks task)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }





        public static List<Tasks> GetTasksForUser( string currentUserId) {
            using (var context = new ApplicationDbContext())
            {
               
                    //They share same list that is filtered by two conditions:
                    //1. Task must have no assignee
                    //2. Task must be assigned to him
                    var tasks = context.Tasks.Where(t => (t.UserId == currentUserId || t.UserId == null)).Include(t => t.Project).Include(t => t.Status).Include(t => t.User).ToList();
                    return tasks;
                
            }

        }
    



        public static Tasks GetTaskById(int taskId)
        {
            using (var context = new ApplicationDbContext())
            {
                var task = context.Tasks.Include(t => t.Project).Include(t => t.Status).Include(t => t.User).FirstOrDefault(t => t.Id == taskId);
                return task;
            }
        }


        public static void EditTask(Tasks task)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(task).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static int GetNumberOfTasksOfUser(String userId) {
            var counter = 0;
            using (var context = new ApplicationDbContext())
            {
                //get tasks that are not finished
                var tasks = context.Tasks.Where(t => t.StatusId != 3).ToList();
                foreach (var task in tasks)
                {
                    if (task.UserId == userId)
                    {
                        counter += 1;
                    }
                     
                }
            }
            return counter;
        }



        public static void DeleteTask(Tasks task)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(task).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }


        public static List<Tasks> GetAllTasks()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Tasks.Include(t => t.Project).Include(t => t.Status).Include(t => t.User).ToList();
            }
        }



        public static List<ApplicationUser> GetUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var users = context.Users.ToList();

                return users;
            }
        }

        public static String GetRoleByUserId(String userId)
        {
            using (var context = new ApplicationDbContext())
            {

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var rolesList = userManager.GetRoles(userId);
                var result = rolesList[0].ToString();
                return result;

            }
        }

        public static ApplicationUser GetUserById(String id)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Find(id);
                return user;
            }
        }

        public static ApplicationUser GetUserByUserName(String userName)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault( u => u.UserName == userName);
                return user;
            }
        }


        public static void DeleteUser(ApplicationUser user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(user).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }


        public static void EditUser(ApplicationUser user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }




        public static List<SelectListItem> GetRolesSelectList()
        {
            var result = new List<SelectListItem>();
            using (var context = new ApplicationDbContext())
            {

                var roles = context.Roles.ToList();

                foreach (var role in roles)
                {
                    var item = new SelectListItem();
                    item.Value = role.Name;
                    item.Text = role.Name;
                    result.Add(item);
                }

            }

            return result;
        }


        public static void ChangeRoleOfUser(string userId, string currentRole, string newRole)
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.RemoveFromRole(userId, currentRole);
                userManager.AddToRole(userId, newRole);
            }
        }

        //public static void AddUser(ApplicationUser user)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        context.Users.Add(user);
        //        context.SaveChanges();
        //    }
        //}

        public static void AddUser (ApplicationUser user, string password, string roleName)
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var result = userManager.Create(user, password);
                if (result.Succeeded) {
                    userManager.AddToRole(user.Id, roleName);
                }
                
            }
        }
        


    }
}