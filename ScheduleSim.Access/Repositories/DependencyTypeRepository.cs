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
            var rows = conn.Query<DependencyType>(@"
                select
                    DependencyTypeCd = @DEPENDENCY_TYPE_CD,
                    DependencyName = @DEPENDENCY_NAME
                from 
                    M_DEPENDENCY_TYPE
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<DependencyType> dependencyTypes)
        {
            var conn = this.connectionFactory.Create();
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
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_DEPENDENCY_TYPE
            ");
        }
    }
}
