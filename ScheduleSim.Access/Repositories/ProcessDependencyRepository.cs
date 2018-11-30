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
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<ProcessDependency>(@"
                select
                    ORG_PROCESS_CD as OrgProcessCd,
                    DST_PROCESS_CD as DstProcessCd,
                    DEPENDENCY_TYPE_CD as DependencyType
                from 
                    M_PROCESS_DEPENDENCY
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<ProcessDependency> dependencies)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_PROCESS_DEPENDENCY (
                    ORG_PROCESS_CD, DST_PROCESS_CD, DEPENDENCY_TYPE_CD
                )
                values (@Org, @Dst, @TypeCd)
            "
            , dependencies
            .Where(x => x.OrgProcessCd != null && x.DstProcessCd != null && x.DependencyType != null)
            .Select(x => new {
                Org = x.OrgProcessCd,
                Dst = x.DstProcessCd,
                TypeCd = (int)x.DependencyType,
            }).ToArray()
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_PROCESS_DEPENDENCY
            "
            , null, trn);
        }
    }
}
