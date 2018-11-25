using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.ProjectSettingPage
{
    public class HolidayChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public HolidayChangeCommand(
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
            var viewModel = sender.DataContext as ProjectSettingPageHolidayItemViewModel;
            var targetHoliday = appContext.Holidays.FirstOrDefault(x => x.HolidayCd == viewModel.Id);
            if (targetHoliday != null)
            {
                targetHoliday.HolidayDate = (e.OriginalSource as DatePicker).SelectedDate;
            }
            e.Handled = true;
        }
    }
}
