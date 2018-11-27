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
    public class LeaveDateChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public LeaveDateChangeCommand(
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
            var targetLeaveDate = appContext.Members.FirstOrDefault(x => x.MemberCd == viewModel.No);
            if (targetLeaveDate != null)
            {
                targetLeaveDate.LeaveDate = (e.OriginalSource as DatePicker).SelectedDate;
            }
            e.Handled = true;
        }
    }
}
