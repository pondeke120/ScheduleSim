using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class CheckependencyOutput
    {
        public IEnumerable<Task> InvalidStartStartDependencies { get; set; }
        public IEnumerable<Task> InvalidStartFinishDependencies { get; set; }
        public IEnumerable<Task> InvalidFinishStartDependencies { get; set; }
        public IEnumerable<Task> InvalidFinishFinishDependencies { get; set; }
    }
}
