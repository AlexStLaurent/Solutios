using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutios.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        [Required]
        [MinLength(2)]
        public string UserName { get; set; }
        [Required]
        [MinLength(2)]
        public string UserFirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string UserEmail { get; set; }
        [Required]
        [MinLength(2)]
        public string UserRole { get; set; }
        [Required]

        public string UserMdp { get; set; }

        public string UserPhone { get; set; }

        public string UserAddress { get; set; }

        [DisplayFormat(NullDisplayText ="Aucune")]
        public string UserAddress2 { get; set; }
       
        
        [DisplayFormat(NullDisplayText = "Non Défini")]
        public string UserZipcode { get; set; }
        
        
        [DisplayFormat(NullDisplayText = "Non Définie")]
        public string UserCity { get; set; }
        
        [DisplayFormat(NullDisplayText = "Non Définie")]
        public string UserProvince { get; set; }
        public string UserProjet { get; set; }
    }
}
