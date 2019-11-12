using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models.ViewModel
{
    public class ViewProjection
    {
        string spending { get; set; }        
        double depenceP { get; set; }
        double budgetestime { get; set; }
        double depensefinalE { get; set; }

        double depenseFinaleEE()
        {
            return (budgetestime + depenceP);
        }

    }
}
