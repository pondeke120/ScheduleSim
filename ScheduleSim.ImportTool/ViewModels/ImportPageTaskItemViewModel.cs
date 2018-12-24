using Prism.Mvvm;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.ImportTool.ViewModels
{
    public class ImportPageTaskItemViewModel : BindableBase
    {
        private int _no;
        public int No
        {
            get { return _no; }
            set { SetProperty(ref _no, value); }
        }

        private string _process;
        public string Process
        {
            get { return _dataContext?.Process; }
            set { SetProperty(ref _process, value); }
        }

        private string _function;
        public string Function
        {
            get { return _dataContext?.Function; }
            set { SetProperty(ref _function, value); }
        }

        private string _taskName;
        public string TaskName
        {
            get { return _dataContext?.TaskName; }
            set { SetProperty(ref _taskName, value); }
        }

        private double? _planValue;
        public double? PlanValue
        {
            get { return _dataContext?.PlanValue; }
            set { SetProperty(ref _planValue, value); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _dataContext?.StartDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _dataContext?.EndDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private OpenFileOutput.TaskItem _dataContext;
        public OpenFileOutput.TaskItem DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
