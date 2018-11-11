using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class FunctionDependencyPageViewModel : BindableBase
    {
        public ICommand DeleteDependencyCommand { get; private set; }

        private List<FunctionDependencyPageDependencyItemViewModel> _dependencies;
        public List<FunctionDependencyPageDependencyItemViewModel> Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        public FunctionDependencyPageViewModel(
            ICommand deleteDependencyCommand)
        {
            DeleteDependencyCommand = deleteDependencyCommand;

            Dependencies = new List<FunctionDependencyPageDependencyItemViewModel>()
            {
                new FunctionDependencyPageDependencyItemViewModel() { SrcFunctionId = 1, DstFunctionId = 2, DependencyType = Entities.Enum.DependencyTypes.CompleteDependency }
            };
        }
    }
}
