using ScheduleSim.Core.BusinessLogics.WPF.Menu;
using ScheduleSim.Core.IO.WPF.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.Menu
{
    public class CreateNewProjectCommand : ICommand
    {
        private ICreateNewProjectBusinessLogic businessLogic;

        public event EventHandler CanExecuteChanged;

        public CreateNewProjectCommand(
            ICreateNewProjectBusinessLogic businessLogic)
        {
            this.businessLogic = businessLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.businessLogic.Execute(new CreateNewProjectInput());
        }
    }
}
