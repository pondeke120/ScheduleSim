using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class SidebarItemViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _viewName;
        public string ViewName
        {
            get { return _viewName; }
            set { SetProperty(ref _viewName, value); }
        }

        private object _bindingView;
        public object BindingView
        {
            get { return _bindingView; }
            set { SetProperty(ref _bindingView, value); }
        }

        private List<SidebarItemViewModel> _children;
        public List<SidebarItemViewModel> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }
    }
}
