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
        [Required]
        [MinLength(2)]
        public string UserPhone { get; set; }
        [Required]
        [MinLength(2)]
        public string UserAddress { get; set; }
        public string UserAddress2 { get; set; }
        [Required]
        [MinLength(2)]
        public string UserZipcode { get; set; }
        [Required]
        [MinLength(2)]
        public string UserCity { get; set; }
        [Required]
        public string UserProvince { get; set; }
        public string UserProjet { get; set; }
    }
}
