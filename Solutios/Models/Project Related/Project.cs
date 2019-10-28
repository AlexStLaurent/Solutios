using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutios.Models
{
    //Project = un projet spécifique
    //ProjectFollowUP = une «projection» d'un projet à un moment spécifique(date)
    //ProjectExpense = une liste des dépenses à un moment spécifique(date)

    public partial class Project
    {
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public DateTime? ProjectDebut { get; set; }

        public DateTime? ProjectFin { get; set; }

        public int? ProjectStatus { get; set; }

        [Required]
        public string jsonProjectSubmission { get; set; }  

        //public string jsonProjectFollowUp { get; set; }//<-- va etre fait dans une table qui va utiliser 2 foreign keys
    }

    public partial class ProjectFollowUp
    {
        public int FollowUp_ID { get; set; }

        public DateTime? FollowUp_Date { get; set; }

        public string jsonFollowUp_Data { get; set; }
    }

    public partial class ProjectExpense
    {
        public int Expensse_ID { get; set; }

        public DateTime? Expense_Date { get; set; }

        public string jsonExpense_Data { get; set; }
    }


}
