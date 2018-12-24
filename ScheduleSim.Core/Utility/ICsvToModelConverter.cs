using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Utility
{
    public interface ICsvToModelConverter<T>
    {
        void SettingHeader(string header);
        T Convert(string text);
    }
}
