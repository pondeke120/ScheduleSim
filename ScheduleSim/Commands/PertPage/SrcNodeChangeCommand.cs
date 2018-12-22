using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertPage
{
    public class SrcNodeChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public SrcNodeChangeCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            var sender = (parameter as object[])[0] as TextBox;
            var e = (parameter as object[])[1] as TextChangedEventArgs;
            var viewModel = sender.DataContext as PertPageEdgeItemViewModel;
            var targetEdge = appContext.PertEdges.FirstOrDefault(x => x.Id == viewModel?.Id);

            if (targetEdge != null)
            {
                var val = 0;
                if (int.TryParse(sender.Text, out val))
                {
                    targetEdge.SrcNodeCd = val;
                }
            }
            e.Handled = true;
        }
    }
}
