using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities;
using Dapper;

namespace ScheduleSim.Access.Repositories
{
    public class DependencyTypeRepository : IDependencyTypeRepository
    {
        private IDbConnectionFactory connectionFactory;

        public DependencyTypeRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<DependencyType> Find()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<DependencyType>(@"
                select
                    DEPENDENCY_TYPE_CD as DependencyTypeCd,
                    DEPENDENCY_NAME as DependencyName
                from 
                    M_DEPENDENCY_TYPE
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<DependencyType> dependencyTypes)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_DEPENDENCY_TYPE (
                    DEPENDENCY_TYPE_CD, DEPENDENCY_NAME
                )
                values (
                    @TypeCd, @Name
                )
            ",
            dependencyTypes.Select(x =>
                new {TypeCd = x.DependencyTypeCd, Name = x.DependencyName }
            ).ToArray()
            , trn);
        }

        public void Upsert(IEnumerable<DependencyType> dependencyTypes)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();

            var exists = Find().Select(x=>x.DependencyTypeCd).ToArray();
            var newRecords = dependencyTypes.Where(x => exists.Contains(x.DependencyTypeCd) == false);

            conn.Execute(@"
                insert into M_DEPENDENCY_TYPE (
                    DEPENDENCY_TYPE_CD, DEPENDENCY_NAME
                )
                values (
                    @TypeCd, @Name
                )
            ",
            newRecords.Select(x =>
                new { TypeCd = x.DependencyTypeCd, Name = x.DependencyName }
            ).ToArray()
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_DEPENDENCY_TYPE
            ", null, trn);
        }
    }
}
