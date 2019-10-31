using Newtonsoft.Json;
using Solutios.Models.Project_Related;
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

        public List<FollowInfo> listProjectSoumission()
        {
            List<FollowInfo> followInfos = new List<FollowInfo>();
            if (ProjectSoumission != null)
            {
                followInfos = JsonConvert.DeserializeObject<List<FollowInfo>>(ProjectSoumission);
            }

            return followInfos;
        }

        public ICollection<ProjectExpense> ProjectExpense { get; set; }
        public ICollection<ProjectFollowUp> ProjectFollowUp { get; set; }
    }
}
