using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;

namespace ScheduleSim.Commands.WbsPage
{
    public class InsertTaskCommand : ICommand
    {
        private AppContext appContext;
        private IIDGenerator taskIdGen;

        public event EventHandler CanExecuteChanged;

        public InsertTaskCommand(
            AppContext appContext,
            IIDGenerator taskIdGen)
        {
            this.appContext = appContext;
            this.taskIdGen = taskIdGen;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // PertPageEdgeItemViewModel
            var viewModels = (parameter as IList).Cast<object>();
            var selectedTop = viewModels.FirstOrDefault(x => x is WbsPageTaskItemViewModel) as WbsPageTaskItemViewModel;
            if (selectedTop != null)
            {
                var posItem = this.appContext.Tasks.FirstOrDefault(x => x.TaskCd == selectedTop.No);
                var index = this.appContext.Tasks.IndexOf(posItem);
                var insertEdges = viewModels.Select(x => new Task()
                {
                    TaskCd = this.taskIdGen.CreateNewId(),
                    ProcessCd = null,
                    FunctionCd = null,
                    PlanValue = null,
                    StartDate = null,
                    EndDate = null,
                    TaskName = null,
                    AssignMemberCd = null,
                });
                this.appContext.Tasks.InsertRange(index, insertEdges);
            }
            else
            {
                // 末尾に一つ追加
                this.appContext.Tasks.AddRange(new[] { new Task()
                {
                    TaskCd = this.taskIdGen.CreateNewId(),
                    ProcessCd = null,
                    FunctionCd = null,
                    PlanValue = null,
                    StartDate = null,
                    EndDate = null,
                    TaskName = null,
                    AssignMemberCd = null,
                }});
            }
        }
    }
}
