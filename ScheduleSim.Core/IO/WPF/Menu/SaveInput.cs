﻿using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;

namespace ScheduleSim.Core.IO.WPF.Menu
{
    public class SaveInput
    {
        public string masterDbFile;
        public string currentDbFile;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<Process> Processes { get; set; }
        public IEnumerable<Function> Functions { get; set; }
        public IEnumerable<Holiday> Holidays { get; set; }
        public IEnumerable<WeekDay> RestDays { get; set; }
        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<ProcessDependency> ProcessDependencies { get; set; }
        public IEnumerable<FunctionDependency> FunctionDependencies { get; set; }
    }
}
