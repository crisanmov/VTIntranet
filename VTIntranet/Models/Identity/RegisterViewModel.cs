using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VTIntranet.Models.Identity
{
    public class RegisterViewModel
    {
        public int idAttachment { get; set; }
        public int idTag { get; set; }
        public string tagName { get; set; }
        public bool attachmentTagsActive { get; set; }

    }

    public class RegisterUserViewModel
    {
        [Display(Name = "idUser")]
        public int idUser { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "UserNameReal")]
        public string UserNameReal { get; set; }

        [Display(Name = "userFirstLastName")]
        public string userFirstLastName { get; set; }

        [Display(Name = "userSecondLastName")]
        public string userSecondLastName { get; set; }

        [Display(Name = "userEmail")]
        [DataType(DataType.EmailAddress)]
        public string userEmail { get; set; }

        [Display(Name = "idProfile")]
        public int idProfile { get; set; }

        [Display(Name = "userActive")]
        public bool? userActive { get; set; }

        [Display(Name = "IdRelationship")]
        public int? IdRelationship { get; set; }

        [Display(Name = "userTypeRelationship")]
        public int userTypeRelationship { get; set; }

        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }
    }

}