using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;

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
            var viewModels = (parameter as IList).Cast<object>()
                .Where(x => x is MemberPageMemberItemViewModel)
                .Cast<MemberPageMemberItemViewModel>().ToArray();
            var ids = viewModels.Select(x => x.No).ToArray();
            var removeMembers = this.appContext.Members.Where(x => ids.Contains(x.MemberCd)).ToArray();
            if (removeMembers.Length > 0)
            {
                this.appContext.Members.RemoveRange(removeMembers);
            }
            // 関連するTaskのMemberIDをnull設定
            foreach (var task in this.appContext.Tasks.Where(x => x.AssignMemberCd != null && ids.Contains(x.AssignMemberCd.Value)))
            {
                task.AssignMemberCd = null;
            }
        }
    }
}
