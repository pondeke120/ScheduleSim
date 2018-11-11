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
            var rows = conn.Query<FunctionDependency>(@"
                select
                    OrgFunctionCd = @ORG_FUNCTION_CD,
                    DstFunctionCd = @DST_FUNCTION_CD,
                    DependencyTypeCd = @DEPENDENCY_TYPE_CD
                from 
                    M_FUNCTION_DEPENDENCY
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<FunctionDependency> dependencies)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_FUNCTION_DEPENDENCY (
                    ORG_FUNCTION_CD, DST_FUNCTION_CD, DEPENDENCY_TYPE_CD
                )
                values (@Org, @Dst, @TypeCd)
            "
            , dependencies.Select(x => new {
                Org = x.OrgFunctionCd,
                Dst = x.DstFunctionCd,
                TypeCd = (int)x.DependencyTypeCd,
            }).ToArray()
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_FUNCTION_DEPENDENCY
            "
            );
        }
    }
}
