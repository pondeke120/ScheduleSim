using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Utility
{
    public interface IIDGenerator
    {
        int CreateNewId();
        void SetCurrentIndex(int index);
        void Reset();
        int CurrentIndex { get; }
    }
}
