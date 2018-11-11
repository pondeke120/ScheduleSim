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
    public class HolidayRepository : IHolidayRepository
    {
        private IDbConnectionFactory connectionFactory;

        public HolidayRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Holiday> Find()
        {
            var conn = this.connectionFactory.Create();
            var rows = conn.Query<Holiday>(@"
                select
                    HolidayCd = @HOLIDAY_CD,
                    HolidayDate = @HOLIDAY_DATE,
                    HolidayName = @HOLIDAY_NAME
                from 
                    M_HOLIDAY
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Holiday> holidays)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_HOLIDAY (
                    HOLIDAY_CD, HOLIDAY_DATE, HOLIDAY_NAME
                )
                values (@Cd, @Date, @Name)
            "
            , holidays.Select(x => new {
                Cd = x.HolidayCd,
                Date = x.HolidayDate,
                Name = x.HolidayName,
            }).ToArray()
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_HOLIDAY
            ");
        }
    }
}
