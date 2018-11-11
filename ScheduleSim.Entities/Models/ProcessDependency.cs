using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class ProcessDependency
    {
        public int OrgProcessCd { get; set; }
        public int DstProcessCd { get; set; }
        public DependencyTypes DependencyType { get; set; }
    }
}
