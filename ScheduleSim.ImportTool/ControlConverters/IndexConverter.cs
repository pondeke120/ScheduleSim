using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScheduleSim.ImportTool.ControlConverters
{
    public class IndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = null as int?;
            var obj = values[0];
            var array = values[1] as IList;
            if (obj != null && array != null)
            {
                var index = array.IndexOf(obj);
                if (index > -1)
                {
                    ret = index + 1;
                }
            }

            return ret.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
