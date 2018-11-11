using ScheduleSim.Core.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Dispatcher
{
    public interface IDispatcher
    {
        TOut Dispatch<TIn, TOut>(IBusinessLogic<TIn, TOut> businessLogic, TIn input);
    }
}
