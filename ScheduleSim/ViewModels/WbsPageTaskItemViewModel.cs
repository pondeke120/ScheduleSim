using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class WbsPageTaskItemViewModel : BindableBase
    {
        private int? _no;
        public int? No
        {
            get { return _no; }
            set { SetProperty(ref _no, value); }
        }

        private int? _processId;
        public int? ProcessId
        {
            get { return _processId; }
            set { SetProperty(ref _processId, value); }
        }

        private int? _functionId;
        public int? FunctionId
        {
            get { return _functionId; }
            set { SetProperty(ref _functionId, value); }
        }

        private string _taskName;
        public string TaskName
        {
            get { return _taskName; }
            set { SetProperty(ref _taskName, value); }
        }

        private double? _pv;
        public double? PV
        {
            get { return _pv; }
            set { SetProperty(ref _pv, value); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        
        private int? _assignMemberId;
        public int? AssignMemberId
        {
            get { return _assignMemberId; }
            set { SetProperty(ref _assignMemberId, value); }
        }
    }
}
