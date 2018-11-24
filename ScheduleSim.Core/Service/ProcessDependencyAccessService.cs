using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;

namespace ScheduleSim.Core.Service
{
    public class ProcessDependencyAccessService : IProcessDependencyAccessService
    {
        private IProcessDependencyRepository processDependencyRepository;

        public ProcessDependencyAccessService(
            IProcessDependencyRepository processDependencyRepository)
        {
            this.processDependencyRepository = processDependencyRepository;
        }

        public IEnumerable<ProcessDependency> GetAllDependencies()
        {
            return
                this.processDependencyRepository.Find();
        }

        public void Update(IEnumerable<ProcessDependency> dependencies)
        {
            this.processDependencyRepository.RemoveAll();
            this.processDependencyRepository.Insert(dependencies);
        }
    }
}
