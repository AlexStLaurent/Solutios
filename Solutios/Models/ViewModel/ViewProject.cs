using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models.ViewModel
{
    public class ViewProject
    {
        public ViewProject(string spending, double soumission, double dépensecourrante, double depenceP)
        {
            this.spending = spending;
            this.soumission = soumission;
            this.dépensecourrante = dépensecourrante;
            this.depenceP = depenceP;
        }

        public string spending { get; set; }
        public double soumission { get; set;}
        public double dépensecourrante { get; set; }
        public double depenceP { get; set; }
        public double budgetestime
        {
            get { return (soumission - (dépensecourrante + depenceP)); }
            set { }
        }
        public double depenseFinaleE { 
            get { return (budgetestime + depenceP); } 
            set { }
        }        

        public double budgetcourant
        {
           get{ return (soumission - dépensecourrante); }
           set{ }
        }
        

    }
}
