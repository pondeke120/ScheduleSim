using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class WeekDay
    {
        public DayOfWeek WeekdayCd { get; set; }
        public string WeekdayName { get; set; }
        public bool HolidayFlg { get; set; }
    }
}
