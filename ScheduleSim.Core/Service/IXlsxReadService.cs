using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IXlsxReadService<T>
    {
        IEnumerable<T> Read(string filePath);
    }
}
