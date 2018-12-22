using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScheduleSim.ControlConverters
{
    public class PertPageTaskNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int?)value;
            var source = (IEnumerable<PertPageTaskItemViewModel>)((CollectionViewSource)parameter).Source;

            var valueItem = source.FirstOrDefault(x => x.TaskId == val);
            return valueItem?.TaskName ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
