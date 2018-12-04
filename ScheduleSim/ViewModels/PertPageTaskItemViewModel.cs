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
        public int TaskId
        {
            get { return _dataContext?.TaskCd ?? -1; }
        }

        public string TaskName
        {
            get { return _dataContext?.TaskName; }
        }

        public int ProcessId
        {
            get { return _dataContext?.ProcessCd ?? -1; }
        }

        public int FunctionId
        {
            get { return _dataContext?.FunctionCd ?? -1; }
        }

        private Task _dataContext;
        public Task DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
