using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutios.Models
{
    //Project = un projet spécifique
    //ProjectFollowUP = une «projection» d'un projet à un moment spécifique(date)
    //ProjectExpense = une liste des dépenses à un moment spécifique(date)

    //modele d'un projet
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
    }


    //suivis|projections d'un projet
    public partial class ProjectFollowUp
    {
        public int FollowUp_ID { get; set; }

        public DateTime? FollowUp_Date { get; set; }

        public string jsonFollowUp_Data { get; set; }
    }


    //depenses d'un projet
    public partial class ProjectExpense
    {
        public int Expense_ID { get; set; }

        public DateTime? Expense_Date { get; set; }

        public string jsonExpense_Data { get; set; }
    }


    //relation entre le projet et ses suivis/projections
    public partial class Project_FollowUp
    {
        public int PF_FollowUp_id { get; set; }
        public int PF_Project_id { get; set; }
    }


    //relation entre le projet et ses dépenses
    public partial class Project_Expense
    {
        public int PE_Expense_id { get; set; }
        public int PE_Project_id { get; set; }
    }

}
