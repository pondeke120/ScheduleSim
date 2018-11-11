using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ProjectSettingPageWeekdayItemViewModel : BindableBase
    {
        private bool _isCheck;
        public bool IsCheck
        {
            get { return this._isCheck; }
            set { SetProperty(ref _isCheck, value); }
        }

        private DayOfWeek _dayOfWeek;
        public DayOfWeek DayOfWeek
        {
            get { return _dayOfWeek; }
            set { SetProperty(ref _dayOfWeek, value); }
        }
    }
}
