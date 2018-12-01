using Prism.Mvvm;
using ScheduleSim.Entities.Enum;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class FunctionDependencyPageDependencyTypeItemViewModel : BindableBase
    {
        public DependencyTypes? DependencyType
        {
            get { return _dataContext?.DependencyTypeCd; }
        }

        public string DependencyTypeName
        {
            get { return _dataContext?.DependencyName; }
        }

        private DependencyType _dataContext;
        public DependencyType DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
