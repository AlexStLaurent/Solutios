using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Solutios.Models.ViewModel
{
    public class ViewIndex
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectDebut { get; set; }
        public DateTime ProjectFin { get; set; }
        public string echeance { get; set; }

        public double jourtotal()
        {
            if (ProjectDebut != null && ProjectFin != null)
            {
                double calcBusinessDays =
                    1 + ((ProjectFin - ProjectDebut).TotalDays * 5 -
                    (ProjectDebut.DayOfWeek - ProjectFin.DayOfWeek) * 2) / 7;

                if (ProjectFin.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
                if (ProjectDebut.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

                return Math.Round(calcBusinessDays, 2);
            }
            return 0;
        }
        public double jourToday()
        {
            if (ProjectFin != null)
            {
                double CalcBusinessDayTotal = 1 + ((ProjectFin - ProjectDebut).TotalDays * 5 -
                    (ProjectDebut.DayOfWeek - ProjectFin.DayOfWeek) * 2) / 7;

                double calcBusinessDays = 
                    1 + ((ProjectFin - DateTime.Now).TotalDays * 5 -
                    (DateTime.Now.DayOfWeek - ProjectFin.DayOfWeek) * 2) / 7;

                if (ProjectFin.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

                return Math.Round( CalcBusinessDayTotal - calcBusinessDays); 
            }
            return 0;
        }

        public double complétion()
        {
            return Math.Round(((jourToday() * 100) / jourtotal()), 2); 
        }

        public string Lastupdate { get; set; }

        public string graph { get; set; }
        public string colorgraph { get; set; }
        public string graphSoumis { get; set; }

        public string label { get; set; }

        public double completion { get { return complétion(); } set { } }
        public double today { get { return jourToday(); } set { } }
        public double totaljour { get { return jourtotal(); } set { } }
        public double margesoumis { get; set; }
        public double margeprojeter { get; set; }
    }
}