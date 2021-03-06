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

        public string RoleName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "SurName")]
        public string SurName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



        public UserViewModel(List<SelectListItem> rolesSelectList) {
            this.RolesSelectList = rolesSelectList;
        }

        public UserViewModel()
        {
            
        }
    }
}