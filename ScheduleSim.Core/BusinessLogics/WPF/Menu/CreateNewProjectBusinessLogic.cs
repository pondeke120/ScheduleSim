using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.Menu;
using System.Reflection;

namespace ScheduleSim.Core.BusinessLogics.WPF.Menu
{
    public class CreateNewProjectBusinessLogic : ICreateNewProjectBusinessLogic
    {
        public CreateNewProjectOutput Execute(CreateNewProjectInput input)
        {
            var output = new CreateNewProjectOutput();

            // 新しいプロセスとして起動
            System.Diagnostics.Process.Start(Assembly.GetEntryAssembly().Location);

            return output;
        }
    }
}
