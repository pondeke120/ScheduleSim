using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using System.Collections;
using ScheduleSim.ViewModels;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using ScheduleSim.Core.Utility;

namespace ScheduleSim.Commands.MemberPage
{
    public class InsertMemberCommand : ICommand
    {
        private AppContext appContext;
        private IIDGenerator memberIdGen;

        public event EventHandler CanExecuteChanged;

        public InsertMemberCommand(
            AppContext appContext,
            IIDGenerator memberIdGen)
        {
            this.appContext = appContext;
            this.memberIdGen = memberIdGen;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // MemberPageMemberItemViewModel
            var viewModels = (parameter as IList).Cast<object>();
            var selectedTop = viewModels.FirstOrDefault(x => x is MemberPageMemberItemViewModel) as MemberPageMemberItemViewModel;
            if (selectedTop != null)
            {
                var posItem = this.appContext.Members.FirstOrDefault(x => x.MemberCd == selectedTop.No);
                var index = this.appContext.Members.IndexOf(posItem);
                var insertMembers = viewModels.Select(x => new Member()
                {
                    MemberCd = this.memberIdGen.CreateNewId()
                });
                this.appContext.Members.InsertRange(index, insertMembers);
            }
            else
            {
                // 末尾に一つ追加
                this.appContext.Members.AddRange(new[] { new Member()
                {
                    MemberCd = this.memberIdGen.CreateNewId()
                }});
            }
        }
    }
}
