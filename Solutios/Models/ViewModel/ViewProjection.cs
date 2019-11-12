using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models.ViewModel
{
    public class ViewProjection
    {
        string spending { get; set; }     
        double soumission { get; set;}
        double dépensecourrante { get; set; }
        double depenceP { get; set; }

        double depenseFinaleE()
        {
            return (budgetestime() + depenceP);
        }

        double budgetcourant()
        {
            return (soumission - dépensecourrante);
        }
        double budgetestime()
        {
            return (soumission - (dépensecourrante + depenceP));
        }

    }
}
