using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Function : INotifyPropertyChanged
    {
        private int _functionCd;
        public int FunctionCd
        {
            get { return _functionCd; }
            set { _functionCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FunctionCd))); }
        }

        private string _functionName;
        public string FunctionName
        {
            get { return _functionName; }
            set { _functionName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FunctionName))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
