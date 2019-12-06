using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using Solutios.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<ViewGraph> graphiqueLigne(int id)
        {
            List<ViewGraph> graph = new List<ViewGraph>();
            Project p = solutiosContext.Project.Find(id);
            List<ProjectFollowUp> PfollowUps = solutiosContext.ProjectFollowUp.Where(c => c.PfProjectId == id).ToList();
            List<FollowUp> follows = new List<FollowUp>();
            foreach (var item in PfollowUps)
            {
                follows.Add(solutiosContext.FollowUp.Find(item.PfFollowUpId));
            }
            List<List<FollowInfo>> infos = new List<List<FollowInfo>>();
            foreach (var item in follows)
            {
                List<FollowInfo> f = JsonConvert.DeserializeObject<List<FollowInfo>>(item.FuInfo);
                infos.Add(f);
            }
            int counterColor = 0;
            foreach (var soumis in p.listProjectSoumission())
            {
                if ((soumis.Spending != "MargeSoumis") && (soumis.Spending != "MargeProjeter"))
                {
                    counterColor += 10;
                    ViewGraph vg = new ViewGraph();
                    string test = "[";
                    foreach (var item in infos)
                    {
                        double total = 0;
                        foreach (var items in item)
                        {
                            if (soumis.Spending == items.Spending)
                            {
                                vg.label = items.Spending;
                                vg.color = items.color;
                                total += items.amount;
                            }
                        }
                        test = test + total.ToString() + ",";
                    }
                    string end = "]";
                    test­ += end;
                    vg.data = test;

                    graph.Add(vg);
                }
            }
            return graph;
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

            string[] test = new string[(f.Count-2)];
            foreach (var items in f)
            {
                if ((items.Spending != "MargeSoumis") && (items.Spending != "MargeProjeter"))
                {
                    test[i] = items.Spending;
                    i++;
                }
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
                test = test + "\"" + Convert.ToDateTime(item.FuDate).ToShortDateString() + "\"" + ",";
            }
            string end = "]";
            test­ += end;

            return test;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewGraph graphbar(int id)
        {
            ViewGraph vg = new ViewGraph();
            Project p = solutiosContext.Project.Find(id);
            ProjectFollowUp PfollowUps = solutiosContext.ProjectFollowUp.LastOrDefault(c => c.PfProjectId == id);
            FollowUp follows = solutiosContext.FollowUp.Find(PfollowUps.PfFollowUpId);
            ProjectExpense projectExpense = solutiosContext.ProjectExpense.LastOrDefault(c => c.PeProjectId == id);
            Expense expense = solutiosContext.Expense.Find(projectExpense.PeExpenseId);

            List<FollowInfo> infos = JsonConvert.DeserializeObject<List<FollowInfo>>(follows.FuInfo);
            List<FollowInfo> soumis = p.listProjectSoumission();
            List<ExpenseInfo> expinfo = JsonConvert.DeserializeObject<List<ExpenseInfo>>(expense.JsonExpenseInfo);

            string end = "]";

            string graphprojection = "[";
            foreach (var item in infos)
            {
                if ((item.Spending != "MargeSoumis") && (item.Spending != "MargeProjeter"))
                {
                    graphprojection = graphprojection + item.amount.ToString() + ",";
                }
            }            
            graphprojection += end;

            string graphreel = "[";
            foreach (var item in expinfo)
            {
                if ((item.Spending != "MargeSoumis") && (item.Spending != "MargeProjeter"))
                {
                    graphreel = graphreel + item.amount.ToString() + ",";
                }
            }
            graphreel += end;

            string colorsoumi = "[";
            foreach (var item in soumis)
            {
                if ((item.Spending != "MargeSoumis") && (item.Spending != "MargeProjeter"))
                {
                    
                    colorsoumi = colorsoumi + '"'+item.color+'"' + ",";
                }
            }

            colorsoumi += end;

            string colorbar = "[";
            string colorbarexpense = "[";
            for (int i = 0; i < soumis.Count; i++)
            {
                if ((soumis[i].Spending != "MargeSoumis") && (soumis[i].Spending != "MargeProjeter"))
                {
                    if(soumis[i].amount == infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#f4b30d" + '"' + ",";
                    }
                    else if (soumis[i].amount < infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#e02d1b" + '"' + ",";
                    }
                    else if (soumis[i].amount > infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#1cc88a" + '"' + ",";
                    }

                    if (soumis[i].amount == expinfo[i].amount)
                    {
                        colorbarexpense = colorbarexpense + '"' + "#f4b30d" + '"' + ",";
                    }
                    else if (soumis[i].amount < expinfo[i].amount)
                    {
                        colorbarexpense = colorbarexpense + '"' + "#e02d1b" + '"' + ",";
                    }
                    else if (soumis[i].amount > expinfo[i].amount)
                    {
                        colorbarexpense = colorbarexpense + '"' + "#1cc88a" + '"' + ",";
                    }

                }
            }
            colorbarexpense += end;
            colorbar += end;

            vg.soumission = soumission(id);
            vg.soumissionColor = colorsoumi;
            vg.data = graphprojection;
            vg.color = colorbar;
            vg.dépense = graphreel;
            vg.colordepense = colorbarexpense;
            return vg;
        }

        public string soumission(int id)
        {
            Project p = solutiosContext.Project.Find(id);
            string soumission = "[";
            foreach (var item in p.listProjectSoumission())
            {
                if ((item.Spending != "MargeSoumis") && (item.Spending != "MargeProjeter"))
                {
                    soumission = soumission + item.amount.ToString() + ",";
                }
            }

            string end = "]";
            soumission += end;
            return soumission;
        }

        public FollowUp GetLastProjection(int id)
        {
            if (solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == id) != null)
            {
                ProjectFollowUp p = solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == id);

                return solutiosContext.FollowUp.LastOrDefault(c => c.FuId == p.PfFollowUpId);
            }
            else
            {
                return null;
            }
        }
        //WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP
        public Expense GetLastExpense(int i)
        {
            if (solutiosContext.ProjectExpense.LastOrDefault(e => e.PeProjectId == i) != null)
            {
                ProjectExpense p = solutiosContext.ProjectExpense.LastOrDefault(e => e.PeProjectId == i);

                return solutiosContext.Expense.LastOrDefault(c => c.ExpenseId == p.PeExpenseId);
            }
            else
            {
                return null;
            }
        }
        //WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP//WIP

        public List<ViewProject> viewProjet(int id)
        {
            List<ViewProject> ListVP = new List<ViewProject>();
            Project p = solutiosContext.Project.Find(id);
            ProjectFollowUp pfu = solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == id);
            FollowUp follow = solutiosContext.FollowUp.LastOrDefault(c => c.FuId == pfu.PfFollowUpId);
            List<FollowInfo> soumission = p.listProjectSoumission();
            List<FollowInfo> estimation = follow.deinfo();
            ProjectExpense pex=solutiosContext.ProjectExpense.LastOrDefault(c => c.PeProjectId == id);
            Expense ex = solutiosContext.Expense.LastOrDefault(c => c.ExpenseId == pex.PeExpenseId);
            List<ExpenseInfo> expenseInfos = ex.Listexpenses();
            for (int i = 0; i < estimation.Count; i++)
            {
                if ((soumission[i].Spending != "MargeSoumis") && (soumission[i].Spending != "MargeProjeter"))
                {
                    ViewProject vp = new ViewProject("", 0, 0, 0);
                    vp.spending = soumission[i].Spending;
                    vp.soumission = soumission[i].amount;
                    vp.depenceP = estimation[i].amount;
                    vp.dépensecourrante = expenseInfos[i].amount;
                    ListVP.Add(vp);
                }
            }
            return ListVP;
        }

        public double Getmarge(int id)
        {
            List<FollowInfo> infos = JsonConvert.DeserializeObject<List<FollowInfo>>(GetLastProjection(id).FuInfo);
            foreach (var item in infos)
            {
                if (item.Spending == "MargeSoumis")
                {
                    return item.amount;
                }
            }

            return 0;
        }

        public double GetmargeEstime(int id)
        {
            List<FollowInfo> infos = JsonConvert.DeserializeObject<List<FollowInfo>>(GetLastProjection(id).FuInfo);
            foreach (var item in infos)
            {
                if (item.Spending == "MargeProjeter")
                {
                    return item.amount;
                }
            }

            return 0;
        }
    }
}