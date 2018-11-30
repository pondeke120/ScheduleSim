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
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<Entities.Models.Task>(@"
                select
                    TASK_CD as TaskCd,
                    PROCESS_CD as ProcessCd,
                    FUNCTION_CD as FunctionCd,
                    TASK_NAME as TaskName,
                    PLAN_VALUE as PlanValue,
                    START_DATE as StartDate,
                    END_DATE as EndDate,
                    ASSIGN_MEMBER_CD as AssignMemberCd
                from 
                    M_TASK
            "
            , null, trn);

            return
                rows.ToArray();
        }

        public void Insert(IEnumerable<Entities.Models.Task> tasks)
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
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
            , trn);
        }

        public void RemoveAll()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            conn.Execute(@"
                delete from M_TASK
            "
            , null, trn);
        }

        public int GetCurrentIndex()
        {
            var conn = this.connectionFactory.Create();
            var trn = this.connectionFactory.GetCurrentTransaction();
            var rows = conn.Query<int?>(@"
                select
                    MAX(TASK_CD) as TaskCd
                from 
                    M_TASK
            "
            , null, trn);

            return
                rows.FirstOrDefault() ?? default(int);
        }
    }
}
