using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;

namespace ScheduleSim.Core.Service
{
    public class PertAccessService : IPertAccessService
    {
        private IPertRepository pertRepository;

        public PertAccessService(
            IPertRepository pertRepository)
        {
            this.pertRepository = pertRepository;
        }

        public IEnumerable<Pert> GetAllEdges()
        {
            return
                this.pertRepository.Find();
        }

        public void Update(IEnumerable<Pert> edges)
        {
            this.pertRepository.RemoveAll();
            this.pertRepository.Insert(edges);
        }
    }
}
