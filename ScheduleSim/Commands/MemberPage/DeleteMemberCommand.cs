using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.MemberPage
{
    public class DeleteMemberCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public DeleteMemberCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public bool CanExecute(object parameter)
        {
            return
                true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as MemberPageMemberItemViewModel;
            var removeMember = this.appContext.Members.FirstOrDefault(x => x.MemberCd == viewModel.No);
            if (removeMember != null)
            {
                this.appContext.Members.Remove(removeMember);
            }
        }
    }
}
