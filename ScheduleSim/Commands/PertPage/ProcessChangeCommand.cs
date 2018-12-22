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
    public class ProcessChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public ProcessChangeCommand(
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
            var sender = (parameter as object[])[0] as ComboBox;
            var e = (parameter as object[])[1] as SelectionChangedEventArgs;
            var viewModel = sender.DataContext as PertPageEdgeItemViewModel;
            var targetEdge = appContext.PertEdges.FirstOrDefault(x => x.Id == viewModel?.Id);

            if (targetEdge != null && e.AddedItems.Count > 0)
            {
                // 選択項目の値を設定
                var selectedValue = ((PertPageProcessItemViewModel)(e.AddedItems[0])).ProcessId;
                viewModel.ProcessId = selectedValue;

                // 関数にフィルタ処理？
            }
            e.Handled = true;
        }
    }
}
