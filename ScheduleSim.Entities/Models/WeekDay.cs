using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class WeekDay : INotifyPropertyChanged
    {
        private DayOfWeek _weekdayCd;
        public DayOfWeek WeekdayCd
        {
            get { return _weekdayCd; }
            set { _weekdayCd = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WeekdayCd))); }
        }

        private string _weekdayName;
        public string WeekdayName
        {
            get { return _weekdayName; }
            set { _weekdayName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WeekdayName))); }
        }

        private bool _holidayFlg;
        public bool HolidayFlg
        {
            get { return _holidayFlg; }
            set { _holidayFlg = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HolidayFlg))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
