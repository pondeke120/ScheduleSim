﻿using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class CalcNodeNumberInput
    {
        public IEnumerable<ProcessDependency> ProcessDependencies { get; set; }
        public IEnumerable<FunctionDependency> FunctionDependencies { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
