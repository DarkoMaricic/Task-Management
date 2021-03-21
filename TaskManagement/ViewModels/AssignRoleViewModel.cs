using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;

namespace TaskManagement.ViewModels
{
    public class AssignRoleViewModel
    {
        public string CurrentRole { get; set; }
        public List<SelectListItem> RolesSelectList { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public String NewRole { get; set; }
        public AssignRoleViewModel(string currentRole, List<SelectListItem> rolesSelectList, string userId, string username)
        {
            this.CurrentRole = currentRole;

            //we are setting current role as selected in select list
            rolesSelectList.ForEach(r => r.Selected = (r.Value == currentRole));

            this.RolesSelectList = rolesSelectList;
            this.UserId = userId;
            this.Username = username;
        }
        public AssignRoleViewModel() { }
    }
}