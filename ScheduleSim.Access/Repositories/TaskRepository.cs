using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities;
using Dapper;

namespace ScheduleSim.Access.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private IDbConnectionFactory connectionFactory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Entities.Models.Task> Find()
        {
            var conn = this.connectionFactory.Create();
            var rows = conn.Query<Entities.Models.Task>(@"
                select
                    TaskCd = @TASK_CD,
                    ProcessCd = @PROCESS_CD,
                    FunctionCd = @FUNCTION_CD,
                    TaskName = @TASK_NAME,
                    PlanValue = @PLAN_VALUE,
                    StartDate = @START_DATE,
                    EndDate = @END_DATE,
                    AssignMemberCd = @ASSIGN_MEMBER_CD
                from 
                    M_TASK
            "
            );

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Entities.Models.Task> tasks)
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                insert into M_TASK (
                    TASK_CD, PROCESS_CD, FUNCTION_CD, TASK_NAME, PLAN_VALUE, START_DATE, END_DATE, ASSIGN_MEMBER_CD
                )
                values (@TaskCd, @ProcessCd, @FunctionCd, @TaskName, @PlanValue, @StartDate, @EndDate, @MemberCd)
            "
            , tasks.Select(x => new {
                TaskCd = x.TaskCd,
                ProcessCd = x.ProcessCd,
                FunctionCd = x.FunctionCd,
                TaskName = x.TaskName,
                PlanValue = x.PlanValue,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                MemberCd = x.AssignMemberCd
            }).ToArray()
            );
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            conn.Execute(@"
                delete from M_TASK
            "
            );
        }
    }
}
