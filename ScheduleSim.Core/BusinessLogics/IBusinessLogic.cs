using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.BusinessLogics
{
    public interface IBusinessLogic<in Tin, out TOut>
    {
        TOut Execute(Tin input);
    }
}
