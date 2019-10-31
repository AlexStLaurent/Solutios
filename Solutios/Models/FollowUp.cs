using Newtonsoft.Json;
using Solutios.Models.Project_Related;
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

        public List<FollowInfo> deinfo()
        {
            List<FollowInfo> followInfos = new List<FollowInfo>();
            if (FuInfo != null)
            {
                followInfos = JsonConvert.DeserializeObject<List<FollowInfo>>(FuInfo);
            }

            return followInfos;
        }

        public ICollection<ProjectFollowUp> ProjectFollowUp { get; set; }
    }
}
