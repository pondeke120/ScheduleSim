﻿using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class FunctionDependency
    {
        public int OrgFunctionCd { get; set; }
        public int DstFunctionCd { get; set; }
        public DependencyTypes DependencyTypeCd { get; set; }
    }
}
