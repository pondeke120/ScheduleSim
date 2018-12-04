using Prism.Mvvm;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class PertPageFunctionItemViewModel : BindableBase
    {
        public int FunctionId
        {
            get { return _dataContext?.FunctionCd ?? -1; }
        }

        public string FunctionName
        {
            get { return _dataContext?.FunctionName; }
        }

        private Function _dataContext;
        public Function DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
