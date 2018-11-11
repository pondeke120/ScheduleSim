using ScheduleSim.Core.Utility;
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
    public class AddMemberCommand : ICommand
    {
        private IIDGenerator memberIdGen;

        public AddMemberCommand(
            IIDGenerator memberIdGen)
        {
            this.memberIdGen = memberIdGen;
            // TODO デバッグ用なので削除
            this.memberIdGen.SetCurrentIndex(2);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var args = parameter as AddingNewItemEventArgs;

            args.NewItem = new MemberPageMemberItemViewModel()
            {
                No = this.memberIdGen.CreateNewId()
            };
        }
    }
}
