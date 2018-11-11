using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Task
    {
        public int TaskCd { get; set; }
        public int ProcessCd { get; set; }
        public int FunctionCd { get; set; }
        public string TaskName { get; set; }
        public double? PlanValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AssignMemberCd { get; set; }
    }
}
