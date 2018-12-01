using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using Dapper;
using ScheduleSim.Entities;

namespace ScheduleSim.Access.Repositories
{
    public class FunctionDependencyRepository : IFunctionDependencyRepository
    {
        private IDbConnectionFactory connectionFactory;

        public FunctionDependencyRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<FunctionDependency> Find()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<FunctionDependency>(@"
                select
                    ORG_FUNCTION_CD as OrgFunctionCd,
                    DST_FUNCTION_CD as DstFunctionCd,
                    DEPENDENCY_TYPE_CD as DependencyTypeCd
                from 
                    M_FUNCTION_DEPENDENCY
            ",
            null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<FunctionDependency> dependencies)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_FUNCTION_DEPENDENCY (
                    ORG_FUNCTION_CD, DST_FUNCTION_CD, DEPENDENCY_TYPE_CD
                )
                values (@Org, @Dst, @TypeCd)
            "
            , dependencies
            .Where(x => x.OrgFunctionCd != null && x.DstFunctionCd != null && x.DependencyTypeCd != null)
            .Select(x => new {
                Org = x.OrgFunctionCd,
                Dst = x.DstFunctionCd,
                TypeCd = (int)x.DependencyTypeCd,
            }).ToArray()
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_FUNCTION_DEPENDENCY
            "
            , null, trn);
        }
    }
}
