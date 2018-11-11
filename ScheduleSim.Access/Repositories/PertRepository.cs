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
    public class PertRepository : IPertRepository
    {
        private IDbConnectionFactory connectionFactory;

        public PertRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Pert> Find()
        {
            var conn = this.connectionFactory.Create();
            var rows = conn.Query<Pert>(@"
                select
                    SrcNodeCd = @SRC_NODE_NO,
                    DstNodeCd = @DST_NODE_NO,
                    TaskCd = @TASK_CD
                from 
                    M_PERT
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Pert> edges)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_PERT (
                    SRC_NODE_NO, DST_NODE_NO, TASK_CD
                )
                values (@Src, @Dst, @Task)
            "
            , edges.Select(x => new {
                Src = x.SrcNodeCd,
                Dst = x.DstNodeCd,
                Task = x.TaskCd,
            }).ToArray()
            );
        }
    }
}
