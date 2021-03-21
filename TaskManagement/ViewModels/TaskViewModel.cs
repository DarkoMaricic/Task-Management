using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TaskManagement.Models;

namespace TaskManagement.ViewModels
{
    public class TaskViewModel
    {
        public List<SelectListItem> ProjectsSelectList { get; set; }
        public List<SelectListItem> StatusesSelectList { get; set; }
        public List<SelectListItem> UsersSelectList { get; set; }
        public Tasks Task { get; set; }
        public String ErrorMessage { get; set; }
        public TaskViewModel(List<SelectListItem> projectsSelectList, List<SelectListItem> statusesSelectList, List<SelectListItem> usersSelectList) {
            this.ProjectsSelectList = projectsSelectList;
            this.StatusesSelectList = statusesSelectList;
            this.UsersSelectList = usersSelectList;
        
        }
        public TaskViewModel() { }
        public TaskViewModel(Tasks task) {
            this.Task = task;
        }
        public TaskViewModel(Tasks task,List<SelectListItem> projectsSelectList, List<SelectListItem> statusesSelectList, List<SelectListItem> usersSelectList)
        {
            this.Task = task;
            this.ProjectsSelectList = projectsSelectList;
            this.StatusesSelectList = statusesSelectList;
            this.UsersSelectList = usersSelectList;

        }
    }
}