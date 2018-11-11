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
            var rows = conn.Query<Member>(@"
                select
                    MemberCd = @MEMBER_CD,
                    MemberName = @MEMBER_NAME,
                    JoinDate = @JOIN_DATE,
                    LeaveDate = @LEAVE_DATE
                from 
                    M_MEMBER
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Member> members)
        {
            var conn = this.connectionFactory.Create();
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
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_MEMBER
            "
            );
        }
    }
}
