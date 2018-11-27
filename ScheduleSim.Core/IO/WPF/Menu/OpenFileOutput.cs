using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.IO.WPF.Menu
{
    public class OpenFileOutput
    {
        public bool IsError { get; set; }
        public string FilePath { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<Process> Processes { get; set; }
        public IEnumerable<Function> Functions { get; set; }
        public IEnumerable<Holiday> Holidays { get; set; }
        public IEnumerable<WeekDay> RestDays { get; set; }
        public int MaxMemberId { get; set; }
        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<ProcessDependency> ProcessDependencies { get; set; }
        public IEnumerable<FunctionDependency> FunctionDependencies { get; set; }
        public IEnumerable<Pert> Edges { get; set; }
    }
}
