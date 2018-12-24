using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.IO.WPF.ImportTool
{
    public class CompleteOutput
    {
        public IEnumerable<OpenFileOutput.TaskItem> TaskItems;
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Process> Processes { get; set; }
        public IEnumerable<Function> Functions { get; set; } 
    }
}
