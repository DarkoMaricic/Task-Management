using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;

namespace TaskManagement.ViewModels
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<SelectListItem> UsersSelectList { get; set; }
        public float Progress { get; set; }


        //CONSTRUCTORS:
        public ProjectViewModel(){}

        public ProjectViewModel(Project project, IEnumerable<SelectListItem> usersSelectlist)
        {
            this.Project = project;
            this.UsersSelectList = usersSelectlist;
        }
        public ProjectViewModel(IEnumerable<SelectListItem> usersSelectlist)
        {
            this.UsersSelectList = usersSelectlist;
        }
        public ProjectViewModel(Project project, float progress)
        {
            this.Project = project;
            this.Progress = progress;
        }

    }
}