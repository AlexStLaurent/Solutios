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

        /// <summary>
        /// Trouve un projet avec son ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project getProjet(int id)
        {
            return solutiosContext.Project.Find(id);
        }

        /// <summary>
        /// Ajoute un projet à la DB
        /// </summary>
        /// <param name="p"></param>
        public void addProjet(Project p)
        {
            solutiosContext.Add(p);
            solutiosContext.SaveChanges();
        }

        /// <summary>
        /// Va chercher les informations necessaire pour afficher le graphique a ligne briser
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Liste d'élément ViewGraph </returns>
        public List<ViewGraph> graphiqueLigne(int id)
        {
            List<ViewGraph> graph = new List<ViewGraph>();
            Project p = solutiosContext.Project.Find(id);

            //Creation de liste de liste des informations de PROJECTION
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

            //Creation de liste de liste des informations de DÉPENSE
            List<ProjectExpense> pfExpense = solutiosContext.ProjectExpense.Where(c => c.PeProjectId == id).ToList();
            List<Expense> expenses = new List<Expense>();
            foreach (var exp in pfExpense)
            {
                expenses.Add(solutiosContext.Expense.Find(exp.PeExpenseId));
            }
            List<List<ExpenseInfo>> Exinfo = new List<List<ExpenseInfo>>();
            foreach (var item in expenses)
            {
                List<ExpenseInfo> f = JsonConvert.DeserializeObject<List<ExpenseInfo>>(item.JsonExpenseInfo);
                Exinfo.Add(f);
            }

            //creation de la string pour le graphique
            int counterColor = 0;
            foreach (var soumis in p.listProjectSoumission())
            {
                if ((soumis.Spending != "MargeSoumis") && (soumis.Spending != "MargeProjeter"))
                {
                    counterColor += 10;
                    ViewGraph vg = new ViewGraph();
                    string test = "[";
                    for(int i = 0; i< infos.Count; i++)
                    {
                        double total = 0;
                        double totalsoumis = 0;
                        for (int j = 0; j < infos[i].Count; j++)
                        {
                            if (soumis.Spending == infos[i][j].Spending)
                            {
                                vg.label = infos[i][j].Spending;
                                vg.color = infos[i][j].color;
                                totalsoumis = soumis.amount + totalsoumis;
                                total = infos[i][j].amount + Exinfo[i][j].amount + total;
                                total = (total * 100) / totalsoumis;
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

        /// <summary>
        /// Crée la string pour les labels du graphique fait en JS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Crée la string de date à afficher en JS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Crée les éléments pour afficher les graphiques
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

            string projectionEX = "[";
            for (int i = 0; i < infos.Count; i++)
            {
                if ((infos[i].Spending != "MargeSoumis") && (infos[i].Spending != "MargeProjeter"))
                {
                    double depense = soumis[i].amount - expinfo[i].amount;
                    projectionEX = projectionEX + depense.ToString() + ",";
                }
            }
            projectionEX += end;

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
                    if((soumis[i].amount - expinfo[i].amount) == infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#f4b30d" + '"' + ",";
                    }
                    else if ((soumis[i].amount - expinfo[i].amount) < infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#e02d1b" + '"' + ",";
                    }
                    else if ((soumis[i].amount - expinfo[i].amount) > infos[i].amount)
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
            vg.projectionEX = projectionEX;
            vg.color = colorbar;
            vg.dépense = graphreel;
            vg.colordepense = colorbarexpense;
            return vg;
        }

        /// <summary>
        /// Crée la string de la soumission pour les graphiques en JS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Va chercher les éléments des dernières Projections
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Va chercher les éléments des dernières dépenses
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Va chercher les éléments des projets et le mets dans la classe
        /// approprier pour les affichers par la suite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Va chercher les éléments des projets et de l'historique et le mets dans la classe
        /// approprier pour les affichers par la suite 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projection_id"></param>
        /// <returns></returns>
        public List<ViewProject> viewProjetHistorique(int id, int projection_id)
        {
            List<ViewProject> ListVP = new List<ViewProject>();
            Project p = solutiosContext.Project.Find(id);
            FollowUp follow = solutiosContext.FollowUp.Find(projection_id);
            List<FollowInfo> soumission = p.listProjectSoumission();
            List<FollowInfo> estimation = follow.deinfo();
            ProjectExpense pex = solutiosContext.ProjectExpense.LastOrDefault(c => c.PeProjectId == id);
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

        /// <summary>
        /// Va chercher la valeur de la marge soumis
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Va chercher la valeur de la marge projeter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calcul le nombre de jour avant la fin du projet pour l'afficher en %
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double getCompletion(int id)
        {

            Project p = getProjet(id);

            DateTime pfin = (DateTime)p.ProjectFin;
            DateTime pdebut = (DateTime)p.ProjectDebut;
            double total = 1;
            double today = 1;

            if (pdebut != null && pfin != null)
            {
                double calcBusinessDays =
                    1 + ((pfin - pdebut).TotalDays * 5 -
                    (pdebut.DayOfWeek - pfin.DayOfWeek) * 2) / 7;

                if (pfin.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
                if (pfin.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

                total = Math.Round(calcBusinessDays, 2);
            }


            if (pfin != null)
            {
                double calcBusinessDaysTotal =
                  1 + ((pfin - pdebut).TotalDays * 5 -
                    (pdebut.DayOfWeek - pfin.DayOfWeek) * 2) / 7;

                double calcBusinessDays =
                    1 + ((pfin - DateTime.Now).TotalDays * 5 -
                    (DateTime.Now.DayOfWeek - pfin.DayOfWeek) * 2) / 7;

                if (pfin.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

                today = Math.Round(calcBusinessDaysTotal - calcBusinessDays);
            }

            return Math.Round(((today * 100) / total), 2);

        }

        /// <summary>
        /// Va chercher toute les projections
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FollowUp> GetAllProjection(int id)
        {
            if (solutiosContext.ProjectFollowUp.LastOrDefault(e => e.PfProjectId == id) != null)
            {
                List<ProjectFollowUp> p = solutiosContext.ProjectFollowUp.Where(e => e.PfProjectId == id).ToList<ProjectFollowUp>();

                List<FollowUp> followUps = new List<FollowUp>();
                foreach(var item in p)
                {
                    followUps.Add(solutiosContext.FollowUp.Where(c => c.FuId == item.PfFollowUpId).First());                    
                }
                return followUps;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Va chercher les informations pour afficher les vieux graphiques de l'historique
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fuid"></param>
        /// <returns></returns>
        public ViewGraph OldGraph(int id, int fuid)
        {
            ViewGraph vg = new ViewGraph();
            Project p = solutiosContext.Project.Find(id);
            ProjectFollowUp PfollowUps = solutiosContext.ProjectFollowUp.LastOrDefault(c => c.PfProjectId == id);
            FollowUp follows = solutiosContext.FollowUp.Find(fuid);
            ProjectExpense projectExpense = solutiosContext.ProjectExpense.LastOrDefault(c => c.PeProjectId == id);
            Expense expense = solutiosContext.Expense.Find(projectExpense.PeExpenseId);

            List<FollowInfo> infos = JsonConvert.DeserializeObject<List<FollowInfo>>(follows.FuInfo);
            List<FollowInfo> soumis = p.listProjectSoumission();
            List<ExpenseInfo> expinfo = JsonConvert.DeserializeObject<List<ExpenseInfo>>(expense.JsonExpenseInfo);

            string end = "]";

            string projectionEX = "[";
            for (int i = 0; i < infos.Count; i++)
            {
                if ((infos[i].Spending != "MargeSoumis") && (infos[i].Spending != "MargeProjeter"))
                {
                    double depense = soumis[i].amount - expinfo[i].amount;
                    projectionEX = projectionEX + depense.ToString() + ",";
                }
            }
            projectionEX += end;

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

                    colorsoumi = colorsoumi + '"' + item.color + '"' + ",";
                }
            }

            colorsoumi += end;

            string colorbar = "[";
            string colorbarexpense = "[";
            for (int i = 0; i < soumis.Count; i++)
            {
                if ((soumis[i].Spending != "MargeSoumis") && (soumis[i].Spending != "MargeProjeter"))
                {
                    if ((soumis[i].amount - expinfo[i].amount) == infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#f4b30d" + '"' + ",";
                    }
                    else if ((soumis[i].amount - expinfo[i].amount) < infos[i].amount)
                    {
                        colorbar = colorbar + '"' + "#e02d1b" + '"' + ",";
                    }
                    else if ((soumis[i].amount - expinfo[i].amount) > infos[i].amount)
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
            vg.projectionEX = projectionEX;
            vg.color = colorbar;
            vg.dépense = graphreel;
            vg.colordepense = colorbarexpense;
            return vg;
        }


    }
}