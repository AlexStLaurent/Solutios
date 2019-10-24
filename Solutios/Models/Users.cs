using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutios.Models
{
    public partial class Users
    {
        public int UserId { get; set; }

        [Display(Name = "Nom d'utilisateur")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Prénom")]
        public string UserFirstName { get; set; }
        [Display(Name = "Adresse Courriel")]
        [Required]
        public string UserEmail { get; set; }
        [Display(Name = "Permissions")]
        [Required]
        public string UserRole { get; set; }
        [Display(Name = "Mot de passe")]
        [Required]
        public string UserMdp { get; set; }

        public string UserPhone {get; set; }

        public string UserAddress {get; set; }

        public string UserAddress2 {get; set; }

        public string UserZipcode {get; set;}

        public string UserCity {get; set;}

        public string UserProvince {get; set;}

        public string UserProjet { get; set; }
    }
}
