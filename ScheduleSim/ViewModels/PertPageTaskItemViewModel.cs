using Prism.Mvvm;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.ViewModels
{
    public class PertPageTaskItemViewModel : BindableBase
    {
        public int? TaskId
        {
            get { return _dataContext?.TaskCd; }
        }

        public string TaskName
        {
            get { return _dataContext?.TaskName; }
        }

        public int? ProcessId
        {
            get { return _dataContext?.ProcessCd; }
        }

        public int? FunctionId
        {
            get { return _dataContext?.FunctionCd; }
        }

        private Task _dataContext;
        public Task DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
