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
    public class ProcessRepository : IProcessRepository
    {
        private IDbConnectionFactory connectionFactory;

        public ProcessRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Process> Find()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<Process>(@"
                select
                    PROCESS_CD as ProcessCd,
                    PROCESS_NAME as ProcessName
                from 
                    M_PROCESS
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Process> processes)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_PROCESS (
                    PROCESS_CD,
                    PROCESS_NAME
                )
                values (@Cd, @Name)
            "
            , processes.Select(x => new {
                Cd = x.ProcessCd,
                Name = x.ProcessName
            }).ToArray()
            , trn);
        }
        
        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_PROCESS
            ", null, trn);
        }
    }
}
