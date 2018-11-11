using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class DbVersion
    {
        public int No { get; set; }
        public string Version { get; set; }
        public string Explain { get; set; }
    }
}
