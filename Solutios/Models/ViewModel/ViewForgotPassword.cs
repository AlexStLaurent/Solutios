using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models.ViewModel
{
    public class ViewForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
