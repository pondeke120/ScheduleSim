﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Enum
{
    public enum DependencyTypes : int
    {
        StartStartDependency,
        StartFinishDependency,
        FinishStartDependency,
        FinishFinishDependency
    }
}
