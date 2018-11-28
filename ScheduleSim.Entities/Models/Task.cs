using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Task : INotifyPropertyChanged
    {
        private int _taskCd;
        public int TaskCd
        {
            get { return _taskCd; }
            set { _taskCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskCd))); }
        }

        private int _processCd;
        public int ProcessCd
        {
            get { return _processCd; }
            set { _processCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessCd))); }
        }

        private int _functionCd;
        public int FunctionCd
        {
            get { return _functionCd; }
            set { _functionCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FunctionCd))); }
        }

        private string _taskName;
        public string TaskName
        {
            get { return _taskName; }
            set { _taskName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskName))); }
        }

        private double? _planValue;
        public double? PlanValue
        {
            get { return _planValue; }
            set { _planValue = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanValue))); }
        }

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
            set { _endDate = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate))); }
        }

        private int? _assignMemberCd;
        public int? AssignMemberCd
        {
            get { return _assignMemberCd; }
            set { _assignMemberCd = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AssignMemberCd))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
