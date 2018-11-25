using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Process : INotifyPropertyChanged
    {
        private int _processCd;
        public int ProcessCd
        {
            get { return _processCd; }
            set { _processCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessCd))); }
        }

        private string _processName;
        public string ProcessName
        {
            get { return _processName; }
            set { _processName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessName))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
