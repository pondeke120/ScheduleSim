using Prism.Mvvm;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ProjectSettingPageFunctionItemViewModel : BindableBase
    {
        private int? _id;
        public int? Id
        {
            get { return _dataContext?.FunctionCd; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _dataContext?.FunctionName; }
            set { SetProperty(ref _name, value); }
        }

        private Function _dataContext;
        public Function DataContext
        {
            get { return _dataContext; }
            set
            {
                if (_dataContext != null)
                {
                    _dataContext.PropertyChanged -= _dataContext_PropertyChanged;
                }
                SetProperty(ref _dataContext, value);
                _dataContext.PropertyChanged += _dataContext_PropertyChanged;
            }
        }

        private void _dataContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(_dataContext.FunctionName)))
            {
                Name = _dataContext.FunctionName;
            }
        }
    }
}
