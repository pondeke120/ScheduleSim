using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScheduleSim.ControlConverters
{
    public class MarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = double.Parse(values[0].ToString());
            var top = double.Parse(values[1].ToString());
            var right = double.Parse(values[2].ToString());
            var bottom = double.Parse(values[3].ToString());

            // 補正値の指定があれば演算
            var leftAddition = 0.0;
            var topAddition = 0.0;
            var rightAddition = 0.0;
            var bottomAddition = 0.0;
            if (parameter != null)
            {
                var additionStr = parameter.ToString();
                var split = additionStr.Split(',');
                if (split.Length == 1)
                {
                    var additional = double.Parse(split[0]);
                    leftAddition = additional;
                    topAddition = additional;
                    rightAddition = additional;
                    bottomAddition = additional;
                }
                else if (split.Length == 4)
                {
                    leftAddition = double.Parse(split[0]);
                    topAddition = double.Parse(split[1]);
                    rightAddition = double.Parse(split[2]);
                    bottomAddition = double.Parse(split[3]);
                }
            }

            return
                new Thickness(left + leftAddition, top + topAddition, right + rightAddition, bottom + bottomAddition);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
