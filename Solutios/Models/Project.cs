using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutios.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public DateTime? ProjectDebut { get; set; }
        public DateTime? ProjectFin { get; set; }
        public int? ProjectStatus { get; set; }
        public string jsonProjectSubmission { get; set; }
        public string jsonProjectFollowUp { get; set; }
    }
}
