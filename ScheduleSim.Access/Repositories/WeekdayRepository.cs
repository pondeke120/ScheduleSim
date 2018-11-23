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
    public class WeekdayRepository : IWeekdayRepository
    {
        private IDbConnectionFactory connectionFactory;

        public WeekdayRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<WeekDay> Find()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<WeekDay>(@"
                select
                    WEEKDAY_CD as WeekdayCd,
                    WEEKDAY_NAME as WeekdayName,
                    HOLIDAY_FLG as HolidayFlg
                from 
                    M_WEEKDAY
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<WeekDay> weekDays)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_WEEKDAY (
                    WEEKDAY_CD, WEEKDAY_NAME, HOLIDAY_FLG
                )
                values (@Cd, @Name, @Flg)
            "
            , weekDays.Select(x => new {
                Cd = (int)x.WeekdayCd,
                Name = x.WeekdayName,
                Flg = x.HolidayFlg
            }).ToArray()
            , trn);
        }

        public void Update(IEnumerable<WeekDay> weekDays)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                update M_WEEKDAY
                set
                    WEEKDAY_NAME = @Name,
                    HOLIDAY_FLG = @Flg
                where
                    WEEKDAY_CD = @Cd
            "
            , weekDays.Select(x => new {
                Cd = x.WeekdayCd,
                Name = x.WeekdayName,
                Flg = x.HolidayFlg
            }).ToArray()
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_WEEKDAY
            ", null, trn);
        }
    }
}
