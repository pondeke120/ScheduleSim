using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;

namespace ScheduleSim.Commands.PertPage
{
    public class InsertEdgeCommand : ICommand
    {
        private AppContext appContext;
        private IIDGenerator pertIdGen;

        public event EventHandler CanExecuteChanged;

        public InsertEdgeCommand(
            AppContext appContext,
            IIDGenerator pertIdGen)
        {
            this.appContext = appContext;
            this.pertIdGen = pertIdGen;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // PertPageEdgeItemViewModel
            var viewModels = (parameter as IList).Cast<object>();
            var selectedTop = viewModels.FirstOrDefault(x => x is PertPageEdgeItemViewModel) as PertPageEdgeItemViewModel;
            if (selectedTop != null)
            {
                var posItem = this.appContext.PertEdges.FirstOrDefault(x => x.Id == selectedTop.Id);
                var index = this.appContext.PertEdges.IndexOf(posItem);
                var insertEdges = viewModels.Select(x => new Pert()
                {
                    Id = this.pertIdGen.CreateNewId(),
                    SrcNodeCd = 0,
                    DstNodeCd = 0,
                    TaskCd = null,
                });
                this.appContext.PertEdges.InsertRange(index, insertEdges);
            }
            else
            {
                // 末尾に一つ追加
                this.appContext.PertEdges.AddRange(new[] { new Pert()
                {
                    Id = this.pertIdGen.CreateNewId(),
                    SrcNodeCd = 0,
                    DstNodeCd = 0,
                    TaskCd = null,
                }});
            }
        }
    }
}
