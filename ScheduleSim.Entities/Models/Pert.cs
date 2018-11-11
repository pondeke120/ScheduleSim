using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Pert
    {
        public int SrcNodeCd { get; set; }
        public int DstNodeCd { get; set; }
        public int TaskCd { get; set; }
    }
}
