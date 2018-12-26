using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class ImportFromWbsInput
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Pert> Perts { get; set; }
    }
}
