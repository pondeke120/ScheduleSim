using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class UpdateCalcValuesInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<DayOfWeek> RestDate { get; set; }
        public IEnumerable<DateTime> Holidays { get; set; }
        public IEnumerable<Member> Members { get; set; }
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
