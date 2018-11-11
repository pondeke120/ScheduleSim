using ScheduleSim.Entities;
using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ScheduleSim.Access.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private IDbConnectionFactory connectionFactory;

        public DbVersionRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void Regist(string version, string explain)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_DB_VERSION (
                    VERSION, EXPLAIN
                )
                values (@v, @e)
            "
            , new[] { new { v = version, e = explain } }
            );
        }
    }
}
