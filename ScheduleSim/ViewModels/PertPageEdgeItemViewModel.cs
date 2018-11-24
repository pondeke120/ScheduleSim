using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class PertPageEdgeItemViewModel : BindableBase
    {
        private int? _iNode;
        public int? INode
        {
            get { return _iNode; }
            set { SetProperty(ref _iNode, value); }
        }

        private int? _jNode;
        public int? JNode
        {
            get { return _jNode; }
            set { SetProperty(ref _jNode, value); }
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

        private int? _taskId;
        public int? TaskId
        {
            get { return _taskId; }
            set { SetProperty(ref _taskId, value); }
        }

        private double? _pv;
        public double? PV
        {
            get { return _pv; }
            set { SetProperty(ref _pv, value); }
        }

        private double? _fastestStartValue;
        public double? FastestStartValue
        {
            get { return _fastestStartValue; }
            set { SetProperty(ref _fastestStartValue, value); }
        }

        private double? _latestStartValue;
        public double? LatestStartValue
        {
            get { return _latestStartValue; }
            set { SetProperty(ref _latestStartValue, value); }
        }

        private double? _fastestEndValue;
        public double? FastestEndValue
        {
            get { return _fastestEndValue; }
            set { SetProperty(ref _fastestEndValue, value); }
        }

        private double? _latestEndValue;
        public double? LatestEndValue
        {
            get { return _latestEndValue; }
            set { SetProperty(ref _latestEndValue, value); }
        }

        private double? _totalFloat;
        public double? TotalFloat
        {
            get { return _totalFloat; }
            set { SetProperty(ref _totalFloat, value); }
        }

        private double? _freeFloat;
        public double? FreeFloat
        {
            get { return _freeFloat; }
            set { SetProperty(ref _freeFloat, value); }
        }

        private bool _isCritical;
        public bool IsCritical
        {
            get { return _isCritical; }
            set { SetProperty(ref _isCritical, value); }
        }
    }
}
