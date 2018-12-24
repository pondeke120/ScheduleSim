using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.ImportTool
{
    public class OpenFileOutput
    {
        public IEnumerable<TaskItem> TaskItems;

        public class TaskItem
        {
            public string Process { get; set; }
            public string Function { get; set; }
            public string TaskName { get; set; }
            public double? PlanValue { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Member { get; set; }
        }
    }
}
