using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class DependencyType
    {
        public DependencyTypes DependencyTypeCd { get; set; }
        public string DependencyName { get; set; }
    }
}
