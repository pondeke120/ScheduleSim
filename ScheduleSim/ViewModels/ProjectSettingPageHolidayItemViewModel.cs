using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ProjectSettingPageHolidayItemViewModel : BindableBase
    {
        private int? _id;
        public int? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
    }
}
