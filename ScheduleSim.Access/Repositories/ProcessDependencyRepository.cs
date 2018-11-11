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
    public class ProcessDependencyRepository : IProcessDependencyRepository
    {
        private IDbConnectionFactory connectionFactory;

        public ProcessDependencyRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<ProcessDependency> Find()
        {
            var conn = this.connectionFactory.Create();
            var rows = conn.Query<ProcessDependency>(@"
                select
                    OrgProcessCd = @ORG_PROCESS_CD,
                    DstProcessCd = @DST_PROCESS_CD,
                    DependencyType = @DEPENDENCY_TYPE_CD
                from 
                    M_PROCESS_DEPENDENCY
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<ProcessDependency> dependencies)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_PROCESS_DEPENDENCY (
                    ORG_PROCESS_CD, DST_PROCESS_CD, DEPENDENCY_TYPE_CD
                )
                values (@Org, @Dst, @TypeCd)
            "
            , dependencies.Select(x => new {
                Org = x.OrgProcessCd,
                Dst = x.DstProcessCd,
                TypeCd = (int)x.DependencyType,
            }).ToArray()
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_PROCESS_DEPENDENCY
            "
            );
        }
    }
}
