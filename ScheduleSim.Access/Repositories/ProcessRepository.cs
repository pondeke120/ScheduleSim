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
            var rows = conn.Query<Process>(@"
                select
                    ProcessCd = @PROCESS_CD,
                    ProcessName = @PROCESS_NAME
                from 
                    M_PROCESS
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Process> processes)
        {
            var conn = this.connectionFactory.Create();
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
            );
        }
        
        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_PROCESS
            ");
        }
    }
}
