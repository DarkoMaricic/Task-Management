using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace TaskManagement.ViewModels
{
    public class UserViewModel
    {
        public List<SelectListItem> RolesSelectList { get; set; }
        public ApplicationUser User { get; set; }
        public String RoleName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public String ConfirmPassword { get; set; }



        public UserViewModel(List<SelectListItem> rolesSelectList) {
            this.RolesSelectList = rolesSelectList;
        }

        public UserViewModel(ApplicationUser user, List<SelectListItem> rolesSelectList)
        {
            this.User = user;
            this.RolesSelectList = rolesSelectList;
        }

        public UserViewModel()
        {
            
        }
    }
}