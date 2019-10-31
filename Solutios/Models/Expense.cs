using Newtonsoft.Json;
using Solutios.Models.Project_Related;
using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class Expense
    {
        public Expense()
        {
            ProjectExpense = new HashSet<ProjectExpense>();
        }

        public int ExpenseId { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string ExpenseInfo { get; set; }

        public List<FollowInfo> listProjectSoumission()
        {
            List<FollowInfo> followInfos = new List<FollowInfo>();
            if (ExpenseInfo != null)
            {
                followInfos = JsonConvert.DeserializeObject<List<FollowInfo>>(ExpenseInfo);
            }

            return followInfos;
        }

        public ICollection<ProjectExpense> ProjectExpense { get; set; }
    }
}
