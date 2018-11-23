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
    public class MemberRepository : IMemberRepository
    {
        private IDbConnectionFactory connectionFactory;

        public MemberRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Member> Find()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<Member>(@"
                select
                    MEMBER_CD as MemberCd,
                    MEMBER_NAME as MemberName,
                    JOIN_DATE as JoinDate,
                    LEAVE_DATE as LeaveDate
                from 
                    M_MEMBER
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Member> members)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                insert into M_MEMBER (
                    MEMBER_CD, MEMBER_NAME, JOIN_DATE, LEAVE_DATE
                )
                values (@Cd, @Name, @Join, @Leave)
            "
            , members.Select(x => new {
                Cd = x.MemberCd,
                Name = x.MemberName,
                Join = x.JoinDate?.ToString("yyyy/MM/dd"),
                Leave = x.LeaveDate?.ToString("yyyy/MM/dd"),
            }).ToArray()
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_MEMBER
            "
            , null, trn);
        }
    }
}
