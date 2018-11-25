using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Holiday : INotifyPropertyChanged
    {
        private int _holidayCd;
        public int HolidayCd
        {
            get { return _holidayCd; }
            set { _holidayCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HolidayCd))); }
        }

        private DateTime? _holidayDate;
        public DateTime? HolidayDate
        {
            get { return _holidayDate; }
            set { _holidayDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HolidayDate))); }
        }

        private string _holidayName;
        public string HolidayName
        {
            get { return _holidayName; }
            set { _holidayName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HolidayName))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
