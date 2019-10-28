using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class FollowUp
    {
        public FollowUp()
        {
            ProjectFollowUp = new HashSet<ProjectFollowUp>();
        }

        public int FuId { get; set; }
        public DateTime? FuDate { get; set; }
        public string FuInfo { get; set; }

        public ICollection<ProjectFollowUp> ProjectFollowUp { get; set; }
    }
}
