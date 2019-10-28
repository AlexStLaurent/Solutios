using System;
using System.Collections.Generic;

namespace Solutios.Models
{
    public partial class ProjectExpense
    {
        public int PeId { get; set; }
        public int? PeExpenseId { get; set; }
        public int? PeProjectId { get; set; }

        public Expense PeExpense { get; set; }
        public Project PeProject { get; set; }
    }
}
