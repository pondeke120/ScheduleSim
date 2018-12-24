using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace ScheduleSim.Commands.Menu
{
    public class ImportXlsxCommand : ICommand
    {
        private IUnityContainer container;

        public event EventHandler CanExecuteChanged;

        public ImportXlsxCommand(IUnityContainer container)
        {
            this.container = container;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var sender = parameter as Window;
            var importWindow = this.container.Resolve<ImportTool.Views.Shell>();
            importWindow.Owner = sender;
            importWindow.ShowDialog();
        }
    }
}
