using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.ProjectSettingPage
{
    public class WeekDayChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public WeekDayChangeCommand(
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
            var sender = (parameter as object[])[0] as CheckBox;
            var e = (parameter as object[])[1] as RoutedEventArgs;
            var viewModel = sender.DataContext as ProjectSettingPageWeekdayItemViewModel;
            var targetWeekday = appContext.WeekDays.FirstOrDefault(x => x.WeekdayCd == viewModel.DayOfWeek);
            if (targetWeekday != null)
            {
                targetWeekday.HolidayFlg = viewModel.IsCheck;
            }
            e.Handled = true;
        }
    }
}
