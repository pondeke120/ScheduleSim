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
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<Holiday>(@"
                select
                    HOLIDAY_CD as HolidayCd,
                    HOLIDAY_DATE as HolidayDate,
                    HOLIDAY_NAME as HolidayName
                from 
                    M_HOLIDAY
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Holiday> holidays)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
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
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_HOLIDAY
            ", null, trn);
        }
    }
}
