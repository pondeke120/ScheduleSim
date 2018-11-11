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
    public class PrjSettingsRepository : IPrjSettingsRepository
    {
        private IDbConnectionFactory connectionFactory;

        public PrjSettingsRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public PrjSettings FindRecently()
        {
            var conn = this.connectionFactory.Create();
            var row = conn.QueryFirst<PrjSettings>(@"
                select
                    StartDate = @START_DATE,
                    EndDate = @END_DATE
                from 
                    M_PRJSETTINGS
            "
            );

            return
                row;
        }

        public void UpdateRecently(PrjSettings settings)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                update M_PRJSETTINGS
                set
                    START_DATE = @Start,
                    END_DATE = @End
            "
            , new {
                Start = settings.StartDate,
                End = settings.EndDate
            });
        }
        
        public void Insert(PrjSettings settings)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_PRJSETTINGS (
                    START_DATE, END_DATE
                )
                values (
                    @Start,
                    @End
                )
            "
            , new []
            {
                new
                {
                    Start = settings.StartDate,
                    End = settings.EndDate
                }
            });
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_PRJSETTINGS
            "
            );
        }
    }
}
