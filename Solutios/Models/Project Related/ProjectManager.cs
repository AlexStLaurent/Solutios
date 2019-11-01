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
            Project i = new Project();
            List<FollowInfo> j = new List<FollowInfo>();
            FollowInfo k = new FollowInfo();
            k.amount = 10;
            k.Spending = "test";
            j.Add(k);
            j.Add(k);
            j.Add(k);
            j.Add(k);
            j.Add(k);
            j.Add(k);
            j.Add(k);
            i.ProjectSoumission = JsonConvert.SerializeObject(j);
            solutiosContext.Add(i);
            solutiosContext.SaveChanges();
        }

        public string SerialiselistFollowUp(List<FollowInfo> f)
        {
            return JsonConvert.SerializeObject(f);
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


    }


}
