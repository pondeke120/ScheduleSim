using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class PrjSettings : INotifyPropertyChanged
    {
        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate))); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
