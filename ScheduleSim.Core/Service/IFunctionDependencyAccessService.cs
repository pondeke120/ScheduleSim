﻿using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IFunctionDependencyAccessService
    {
        void Update(IEnumerable<FunctionDependency> dependencies);
        IEnumerable<FunctionDependency> GetAllDependencies();
    }
}
