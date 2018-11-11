using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;

namespace ScheduleSim.Core.Service
{
    public class FunctionDependencyAccessService : IFunctionDependencyAccessService
    {
        private IFunctionDependencyRepository functionDependencyRepository;

        public FunctionDependencyAccessService(
            IFunctionDependencyRepository functionDependencyRepository)
        {
            this.functionDependencyRepository = functionDependencyRepository;
        }

        public IEnumerable<FunctionDependency> GetAllDependencies()
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<FunctionDependency> dependencies)
        {
            this.functionDependencyRepository.RemoveAll();
            this.functionDependencyRepository.Insert(dependencies);
        }
    }
}
