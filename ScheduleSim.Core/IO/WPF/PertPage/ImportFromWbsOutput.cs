﻿using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class ImportFromWbsOutput
    {
        public IEnumerable<Pert> Perts { get; set; }
    }
}
