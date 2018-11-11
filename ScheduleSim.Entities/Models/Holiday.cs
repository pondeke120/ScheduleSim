using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Holiday
    {
        public int HolidayCd { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; }
    }
}
