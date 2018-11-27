using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.MemberPage
{
    public class JoinDateChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public JoinDateChangeCommand(
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
            var sender = (parameter as object[])[0] as DatePicker;
            var e = (parameter as object[])[1] as SelectionChangedEventArgs;
            var viewModel = sender.DataContext as MemberPageMemberItemViewModel;
            var targetJoinDate = appContext.Members.FirstOrDefault(x => x.MemberCd == viewModel.No);
            if (targetJoinDate != null)
            {
                targetJoinDate.JoinDate = (e.OriginalSource as DatePicker).SelectedDate;
            }
            e.Handled = true;
        }
    }
}
