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
        public string JsonExpenseInfo { get; set; }

        /*public List<ExpenseInfo> listProjectSoumission()
        {
            List<ExpenseInfo> expenseInfos = new List<ExpenseInfo>();
            if (JsonExpenseInfo != null)
            {
                expenseInfos = JsonConvert.DeserializeObject<List<ExpenseInfo>>(JsonExpenseInfo);
            }

            return expenseInfos;
        }*/

        public ICollection<ProjectExpense> ProjectExpense { get; set; }
    }
}
