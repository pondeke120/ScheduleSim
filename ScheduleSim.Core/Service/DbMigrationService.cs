using ScheduleSim.Entities.Enum;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public class DbMigrationService : IDbMigrationService
    {
        private IDbVersionRepository dbVersionRepository;
        private IDependencyTypeRepository dependencyTypeRepository;

        public DbMigrationService(
            IDbVersionRepository dbVersionRepository,
            IDependencyTypeRepository dependencyTypeRepository)
        {
            this.dbVersionRepository = dbVersionRepository;
            this.dependencyTypeRepository = dependencyTypeRepository;
        }

        public void Upgrade()
        {
            this.dependencyTypeRepository.Upsert(Enum.GetValues(typeof(DependencyTypes)).Cast<DependencyTypes>().Select(x => new DependencyType() { DependencyTypeCd = x, DependencyName = Enum.GetName(typeof(DependencyTypes), x) }));
            this.dbVersionRepository.Regist("1.0.0.0", "first version");
        }
    }
}
