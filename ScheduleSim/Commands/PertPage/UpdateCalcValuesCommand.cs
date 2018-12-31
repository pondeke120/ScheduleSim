using ScheduleSim.Core.BusinessLogics.WPF.PertPage;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertPage
{
    public class UpdateCalcValuesCommand : ICommand
    {
        private AppContext appContext;
        private IUpdateCalcValuesBusinessLogic businessLogic;

        public event EventHandler CanExecuteChanged;

        public UpdateCalcValuesCommand(
            AppContext appContext,
            IUpdateCalcValuesBusinessLogic businessLogic)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as PertPageViewModel;

            // 基本データの更新
            foreach (var edge in viewModel.Edges)
            {
                var taskId = edge.TaskId;
                var task = this.appContext.Tasks.FirstOrDefault(x => x.TaskCd == taskId);
                if (task != null)
                {
                    edge.ProcessId = task.ProcessCd;
                    edge.FunctionId = task.FunctionCd;
                    edge.PV = task.PlanValue;
                }
                else
                {
                    edge.PV = 0.0;
                }
            }

            // 演算に必要なデータが入力済みであるかをチェックし、不足があれば演算を中止
            if (CheckCalc() == false)
                return;

            // 計算データの更新
            var input = new UpdateCalcValuesInput();
            input.StartDate = appContext.PrjSettings.StartDate.Value;
            input.EndDate = appContext.PrjSettings.EndDate.Value;
            input.RestDate = appContext.WeekDays.Where(x => x.HolidayFlg).Select(x => x.WeekdayCd).ToArray();
            input.Holidays = appContext.Holidays.Where(x => x.HolidayDate.HasValue).Select(x => x.HolidayDate.Value).ToArray();
            input.Members = appContext.Members;
            input.Data = viewModel.Edges
            .Where(x => x.INode.HasValue && x.JNode.HasValue && x.PV.HasValue)
            .Select(x => new UpdateCalcValuesInput.ActivityData()
            {
                Id = x.Id,
                SrcNodeId = x.INode.Value,
                DstNodeId = x.JNode.Value,
                PlanValue = x.PV.Value,
            }).ToArray();
            var output = null as UpdateCalcValuesOutput;
            try
            {
                output = this.businessLogic.Execute(input);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "演算エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var vmEdges = viewModel.Edges.Where(x => x.INode.HasValue && x.JNode.HasValue && x.PV.HasValue).OrderBy(x => x.Id).ToArray();
            var enEdges = output.CalcValues.OrderBy(x => x.EdgeId).ToArray();
            var pairs = vmEdges.Zip(enEdges, (a, b) => new { Vm = a, CalcVal = b }).ToArray();
            foreach (var pair in pairs)
            {
                var vmEdge = pair.Vm;
                var enEdge = pair.CalcVal;

                vmEdge.FastestStartValue = enEdge.EarliestStartTime;
                vmEdge.FastestEndValue = enEdge.EarliestFinishTime;
                vmEdge.LatestStartValue = enEdge.LatestStartTime;
                vmEdge.LatestEndValue = enEdge.LatestFinishTime;
                vmEdge.FreeFloat = enEdge.FreeFloat;
                vmEdge.TotalFloat = enEdge.TotalFloat;
                vmEdge.IsCritical = enEdge.IsCritical;
            }

        }

        /// <summary>
        /// 演算に必要な入力がそろっているか確認する
        /// </summary>
        /// <returns></returns>
        private bool CheckCalc()
        {
            return
                appContext.PrjSettings.StartDate != null
                && appContext.PrjSettings.EndDate != null;
        }
    }
}
