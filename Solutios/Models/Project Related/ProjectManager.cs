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
        private readonly ProjetSolutiosContext solutiosContext = new ProjetSolutiosContext();
                

        public Project getProjet(int id)
        {
            return solutiosContext.Project.Find(id);
        }

        public void addProjet(Project p)
        {
            solutiosContext.Add(p);
            solutiosContext.SaveChanges();
        }

        public string SerialiselistFollowUp(List<Followup> f)
        {
            return JsonConvert.SerializeObject(f);
        }



    }


}
