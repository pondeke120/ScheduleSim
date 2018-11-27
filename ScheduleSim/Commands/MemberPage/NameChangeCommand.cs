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
    public class NameChangeCommand : ICommand
    {
        private AppContext appContext;

        public NameChangeCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var sender = (parameter as object[])[0] as TextBox;
            var e = (parameter as object[])[1] as TextChangedEventArgs;
            var viewModel = sender.DataContext as MemberPageMemberItemViewModel;
            var targetMember = appContext.Members.FirstOrDefault(x => x.MemberCd == viewModel.No);
            if (targetMember != null)
            {
                targetMember.MemberName = (e.OriginalSource as TextBox).Text;
            }
            e.Handled = true;
        }
    }
}
