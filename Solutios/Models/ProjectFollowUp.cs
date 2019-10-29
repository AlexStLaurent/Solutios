using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class ProjectFollowUp
    {
        public int PfId { get; set; }
        public int? PfFollowUpId { get; set; }
        public int? PfProjectId { get; set; }

        public FollowUp PfFollowUp { get; set; }
        public Project PfProject { get; set; }
    }
}
