using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class WbsPageViewModel : BindableBase
    {
        public ICommand DeleteTaskCommand { get; private set; }

        private List<WbsPageTaskItemViewModel> _tasks;
        public List<WbsPageTaskItemViewModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        public WbsPageViewModel(ICommand deleteTaskCommand)
        {
            DeleteTaskCommand = deleteTaskCommand;

            Tasks = new List<WbsPageTaskItemViewModel>()
            {
                new WbsPageTaskItemViewModel() { No = 1, ProcessId = 1, FunctionId = 1, TaskName = "task1", PV = 2.0, StartDate = new DateTime(2018, 10, 2), EndDate = new DateTime(2018, 10, 30), AssignMemberId = 1 },
                new WbsPageTaskItemViewModel() { No = 2, ProcessId = 1, FunctionId = 1, TaskName = "task2", PV = 8.0 },
                new WbsPageTaskItemViewModel() { No = 3, ProcessId = 1, FunctionId = 1, TaskName = "task3" }
            };
        }
    }
}
