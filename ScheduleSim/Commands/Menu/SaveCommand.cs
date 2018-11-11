using ScheduleSim.Core.BusinessLogics.WPF.Menu;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.Menu
{
    public class SaveCommand : ICommand
    {
        private ISaveBusinessLogic businessLogic;
        private AppContext appContext;
        private IDispatcher dispatcher;

        public event EventHandler CanExecuteChanged;

        public SaveCommand(
            AppContext appContext,
            IDispatcher dispacher,
            ISaveBusinessLogic businessLogic)
        {
            this.appContext = appContext;
            this.dispatcher = dispacher;
            this.businessLogic = businessLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("SaveCommand");

            var input = new Core.IO.WPF.Menu.SaveInput();

            input.masterDbFile = this.appContext.MasterDbFile;
            input.currentDbFile = this.appContext.ProjectDbFile;

            this.dispatcher.Dispatch(this.businessLogic, input);
        }
    }
}
