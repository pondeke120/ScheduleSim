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
    public class ProductivityChangeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private AppContext appContext;

        public ProductivityChangeCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

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
                var text = (e.OriginalSource as TextBox).Text;
                var val = 0.0;
                if (double.TryParse(text, out val))
                {
                    targetMember.Productivity = val;
                }
            }
            e.Handled = true;
        }
    }
}
