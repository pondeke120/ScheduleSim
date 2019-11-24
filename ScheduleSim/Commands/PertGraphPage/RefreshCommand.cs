using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertGraphPage
{
    public class RefreshCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public RefreshCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as PertGraphPageViewModel;

            viewModel.RemakeGraph();
        }
    }
}
