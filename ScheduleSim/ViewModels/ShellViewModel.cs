using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return $"ScheduleSim {_projectPath}"; }
            set { SetProperty(ref _title, value); }
        }

        private string _projectPath;
        public string ProjectPath
        {
            get { return _projectPath; }
            set {
                SetProperty(ref _projectPath, value);
                Title = $"ScheduleSim {_projectPath}";
            }
        }
    }
}
