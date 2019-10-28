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

        public ICollection<ProjectExpense> ProjectExpense { get; set; }
    }
}
