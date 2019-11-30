using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class CalcNodeNumberOutput
    {
        public IEnumerable<Pert> PertEdges { get; set; }
    }
}
