using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class UpdateCalcValuesInput
    {
        public IEnumerable<ActivityData> Data { get; set; }

        public class ActivityData
        {
            public int Id { get; set; }
            public int SrcNodeId { get; set; }
            public int DstNodeId { get; set; }
            public double PlanValue { get; set; }
        }
    }
}
