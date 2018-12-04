using Prism.Mvvm;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class PertPageProcessItemViewModel : BindableBase
    {
        public int ProcessId
        {
            get { return _dataContext?.ProcessCd ?? -1; }
        }

        public string ProcessName
        {
            get { return _dataContext?.ProcessName; }
        }

        private Process _dataContext;
        public Process DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
