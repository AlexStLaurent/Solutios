using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models
{
    public class ExpenseInfo
    {
        public string Spending { get; set;}

        public double amount { get; set;}

        public List<FollowInfo> subExpenses { get; set; }

    }
}
