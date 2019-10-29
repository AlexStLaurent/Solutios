using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class Project
    {
        public Project()
        {
            ProjectExpense = new HashSet<ProjectExpense>();
            ProjectFollowUp = new HashSet<ProjectFollowUp>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ProjectDebut { get; set; }
        public DateTime? ProjectFin { get; set; }
        public int? ProjectStatus { get; set; }
        public string ProjectSoumission { get; set; }

        public ICollection<ProjectExpense> ProjectExpense { get; set; }
        public ICollection<ProjectFollowUp> ProjectFollowUp { get; set; }
    }
}
