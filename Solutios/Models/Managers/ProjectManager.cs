using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using Solutios.Models.Project_Related;
using Solutios.Models.ViewModel;
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


        public string tendance(int id)
        {
            Project p = solutiosContext.Project.Find(id);
            List<ProjectFollowUp> PfollowUps = solutiosContext.ProjectFollowUp.Where(c => c.PfProjectId == id).ToList();
            List<FollowUp> follows = new List<FollowUp>();
            foreach(var item in PfollowUps)
            {
                follows.Add(solutiosContext.FollowUp.Find(item.PfFollowUpId));
            }
            string test = "[";
            List<List<FollowInfo>> infos = new List<List<FollowInfo>>();
            foreach(var item in follows)
            {
                List<FollowInfo> f = JsonConvert.DeserializeObject<List<FollowInfo>>(item.FuInfo);
                infos.Add(f);
            }
            foreach (var item in infos)
            {
                foreach(var items in item)
                {
                    if(items.Spending == "RRR")
                    {
                        test = test + items.amount.ToString() + ",";
                    }
                }
            }
            string end = "]";
            test­ += end;

            return test;
        }
        public string nomdépense(int id)
        {
            Project p = solutiosContext.Project.Find(id);
            List<ProjectFollowUp> PfollowUps = solutiosContext.ProjectFollowUp.Where(c => c.PfProjectId == id).ToList();
            FollowUp follows = new FollowUp();
            foreach (var item in PfollowUps)
            {
                follows = solutiosContext.FollowUp.FirstOrDefault(c => c.FuId == item.PfFollowUpId);
            }

            int i = 0;

            List<FollowInfo> f = JsonConvert.DeserializeObject<List<FollowInfo>>(follows.FuInfo);

            string[] test = new string[f.Count];
            foreach (var items in f)
                {

                    test[i] = items.Spending;
                    i++;
                }

            

            string jsonTest = (new JavaScriptSerializer()).Serialize(test);
            return jsonTest;
        }

        public string date(int id)
        {
            Project p = solutiosContext.Project.Find(id);
            List<ProjectFollowUp> PfollowUps = solutiosContext.ProjectFollowUp.Where(c => c.PfProjectId == id).ToList();
            List<FollowUp> follows = new List<FollowUp>();
            foreach (var item in PfollowUps)
            {
                follows.Add(solutiosContext.FollowUp.Find(item.PfFollowUpId));
            }
            string test = "[";
            foreach (var item in follows)
            {
                test = test + "\"" +  Convert.ToDateTime(item.FuDate) + "\"" + ",";
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
                return null;
            }

        }

        public List<ViewProject> viewProjet(int id)
        {
            List<ViewProject> ListVP = new List<ViewProject>();
            Project p = solutiosContext.Project.Find(id);
            ProjectFollowUp pfu = solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == id);
            FollowUp follow = solutiosContext.FollowUp.LastOrDefault(c => c.FuId == pfu.PfFollowUpId);
            List<FollowInfo> soumission = p.listProjectSoumission();
            List<FollowInfo> estimation = follow.deinfo();
            for(int i = 0; i < estimation.Count; i++)
            {
                ViewProject vp = new ViewProject("",0,0,0);
                vp.spending = soumission[i].Spending;
                vp.soumission = soumission[i].amount;
                vp.depenceP = estimation[i].amount;
                ListVP.Add(vp);
            }
            return ListVP;
        }



    }


}
