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
    public class FunctionRepository : IFunctionRepository
    {
        private IDbConnectionFactory connectionFactory;

        public FunctionRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Function> Find()
        {
            var conn = this.connectionFactory.Create();
            var rows = conn.Query<Function>(@"
                select
                    FunctionCd = @FUNCTION_CD,
                    FunctionName = @FUNCTION_NAME
                from 
                    M_FUNCTION
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Function> functions)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_FUNCTION (
                    FUNCTION_CD, FUNCTION_NAME
                )
                values (@Cd, @Name)
            "
            , functions.Select(x => new {
                Cd = x.FunctionCd, Name = x.FunctionName,
            }).ToArray()
            );
        }
        
        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_FUNCTION
            ");
        }
    }
}
