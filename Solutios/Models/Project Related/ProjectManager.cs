using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Solutios.Models.Project_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutios.Models
{
    public class ProjectManager
    {
        private readonly ProjetSolutiosContext solutiosContext;
        private IConfiguration config;
        

        public ProjectManager()
        {

        }

        public ProjectManager(ProjetSolutiosContext context)
        {

            solutiosContext = context;
        }



        public Project getProjet(int id)
        {
            return solutiosContext.Project.Find(id);
        }

        public void addProjet(Project p)
        {            
            solutiosContext.Add(p);
            solutiosContext.SaveChanges();
        }


        public string diagrame(int id)
        {
            Project p = solutiosContext.Project.Find(id);
            string test = "[";
            foreach (var item in p.listProjectSoumission())
            {
                test = test + item.amount.ToString() + ",";
            }
            string end = "]";
            test­ += end;

            return test;
        }

        public FollowUp GetLastProjection(int i)
        {
            if (solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == i) != null)
            {
                ProjectFollowUp p = solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == i);

                return solutiosContext.FollowUp.LastOrDefault(c => c.FuId == p.PfFollowUpId);
            }
            else
            {
                FollowUp f = null;
                
                return f;
            }
            
        }



    }


}
