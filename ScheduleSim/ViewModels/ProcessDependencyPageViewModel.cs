using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class ProcessDependencyPageViewModel : BindableBase
    {
        public ICommand DeleteDependencyCommand { get; private set; }

        private List<ProcessDependencyPageDependencyItemViewModel> _dependencies;
        public List<ProcessDependencyPageDependencyItemViewModel> Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        public ProcessDependencyPageViewModel(
            ICommand deleteDependencyCommand)
        {
            DeleteDependencyCommand = deleteDependencyCommand;

            Dependencies = new List<ProcessDependencyPageDependencyItemViewModel>()
            {
                new ProcessDependencyPageDependencyItemViewModel() { SrcProcessId = 1, DstProcessId = 2, DependencyType = Entities.Enum.DependencyTypes.StartDependency },
            };
        }
    }
}
