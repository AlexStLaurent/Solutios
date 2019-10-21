using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string UserMdp { get; set; }
        public string UserDetail { get; set; }
        public string UserProjet { get; set; }
    }
}
