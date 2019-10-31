using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.Entities.Models;

namespace ScheduleSim.Core.BusinessLogics.WPF.PertPage
{
    public class CheckDependencyBusinessLogic : ICheckDependencyBusinessLogic
    {
        public CheckependencyOutput Execute(CheckDependencyInput input)
        {
            var output = new CheckependencyOutput();
            output.InvalidStartStartDependencies = new Task[0];
            output.InvalidStartFinishDependencies = new Task[0];
            output.InvalidFinishStartDependencies = new Task[0];
            output.InvalidFinishFinishDependencies = new Task[0];
            
            // 工程の依存性チェック
            CheckProcessDependency(input.Tasks, input.ProcessDependencies, output);

            // 機能の依存性チェック
            CheckFunctionDependency(input.Tasks, input.FunctionDependencies, output);

            // 重複タスクを削除
            output.InvalidStartStartDependencies = output.InvalidStartStartDependencies.Distinct();
            output.InvalidStartFinishDependencies = output.InvalidStartFinishDependencies.Distinct();
            output.InvalidFinishStartDependencies = output.InvalidFinishStartDependencies.Distinct();
            output.InvalidFinishFinishDependencies = output.InvalidFinishFinishDependencies.Distinct();

            return output;
        }

        private void CheckProcessDependency(IEnumerable<Task> pertWithTasks, IEnumerable<ProcessDependency> processDependencies, CheckependencyOutput output)
        {
            // 同一機能でグルーピング
            var sameFunctionTasks = pertWithTasks.GroupBy(x => x.FunctionCd);

            foreach (var dependency in processDependencies.Where(x => x.DependencyType == Entities.Enum.DependencyTypes.StartStartDependency))
            {
                var invalidStartStartDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeEarlyDate = x.Where(y => y.ProcessCd == dependency.OrgProcessCd && y.StartDate != null).Min(y => y.StartDate);
                    var dstEdges = x.Where(y => y.ProcessCd == dependency.DstProcessCd && y.StartDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeEarlyDate > z.StartDate);
                });

                output.InvalidStartStartDependencies = output.InvalidStartStartDependencies.Concat(invalidStartStartDependencies.ToArray());
            }


            foreach (var dependency in processDependencies.Where(x => x.DependencyType == Entities.Enum.DependencyTypes.StartFinishDependency))
            {
                var invalidStartFinishDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeEarlyDate = x.Where(y => y.ProcessCd == dependency.OrgProcessCd && y.StartDate != null).Min(y => y.StartDate);
                    var dstEdges = x.Where(y => y.ProcessCd == dependency.DstProcessCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeEarlyDate > z.EndDate);
                });

                output.InvalidStartFinishDependencies = output.InvalidStartFinishDependencies.Concat(invalidStartFinishDependencies.ToArray());
            }


            foreach (var dependency in processDependencies.Where(x => x.DependencyType == Entities.Enum.DependencyTypes.FinishStartDependency))
            {
                var invalidFinishStartDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeLatestDate = x.Where(y => y.ProcessCd == dependency.OrgProcessCd && y.EndDate != null).Max(y => y.EndDate);
                    var dstEdges = x.Where(y => y.ProcessCd == dependency.DstProcessCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeLatestDate > z.StartDate);
                });

                output.InvalidFinishStartDependencies = output.InvalidFinishStartDependencies.Concat(invalidFinishStartDependencies.ToArray());
            }


            foreach (var dependency in processDependencies.Where(x => x.DependencyType == Entities.Enum.DependencyTypes.FinishFinishDependency))
            {
                var invalidFinishFinishDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeLatestDate = x.Where(y => y.ProcessCd == dependency.OrgProcessCd && y.EndDate != null).Max(y => y.EndDate);
                    var dstEdges = x.Where(y => y.ProcessCd == dependency.DstProcessCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeLatestDate > z.EndDate);
                });

                output.InvalidFinishFinishDependencies = output.InvalidFinishFinishDependencies.Concat(invalidFinishFinishDependencies.ToArray());
            }
        }

        private void CheckFunctionDependency(IEnumerable<Task> pertWithTasks, IEnumerable<FunctionDependency> functionDependencies, CheckependencyOutput output)
        {
            // 同一工程でグルーピング
            var sameFunctionTasks = pertWithTasks.GroupBy(x => x.ProcessCd);

            foreach (var dependency in functionDependencies.Where(x => x.DependencyTypeCd == Entities.Enum.DependencyTypes.StartStartDependency))
            {
                var invalidStartStartDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeEarlyDate = x.Where(y => y.FunctionCd == dependency.OrgFunctionCd && y.StartDate != null).Min(y => y.StartDate);
                    var dstEdges = x.Where(y => y.FunctionCd == dependency.DstFunctionCd && y.StartDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeEarlyDate > z.StartDate);
                });

                output.InvalidStartStartDependencies = output.InvalidStartStartDependencies.Concat(invalidStartStartDependencies.ToArray());
            }


            foreach (var dependency in functionDependencies.Where(x => x.DependencyTypeCd == Entities.Enum.DependencyTypes.StartFinishDependency))
            {
                var invalidStartFinishDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeEarlyDate = x.Where(y => y.FunctionCd == dependency.OrgFunctionCd && y.StartDate != null).Min(y => y.StartDate);
                    var dstEdges = x.Where(y => y.FunctionCd == dependency.DstFunctionCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeEarlyDate > z.EndDate);
                });

                output.InvalidStartFinishDependencies = output.InvalidStartFinishDependencies.Concat(invalidStartFinishDependencies.ToArray());
            }


            foreach (var dependency in functionDependencies.Where(x => x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishStartDependency))
            {
                var invalidFinishStartDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeLatestDate = x.Where(y => y.FunctionCd == dependency.OrgFunctionCd && y.EndDate != null).Max(y => y.EndDate);
                    var dstEdges = x.Where(y => y.FunctionCd == dependency.DstFunctionCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeLatestDate > z.StartDate);
                });

                output.InvalidFinishStartDependencies = output.InvalidFinishStartDependencies.Concat(invalidFinishStartDependencies.ToArray());
            }


            foreach (var dependency in functionDependencies.Where(x => x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishFinishDependency))
            {
                var invalidFinishFinishDependencies = sameFunctionTasks.SelectMany(x =>
                {
                    var srcEdgeLatestDate = x.Where(y => y.FunctionCd == dependency.OrgFunctionCd && y.EndDate != null).Max(y => y.EndDate);
                    var dstEdges = x.Where(y => y.FunctionCd == dependency.DstFunctionCd && y.EndDate != null).ToArray();

                    return
                        dstEdges.Where(z => srcEdgeLatestDate > z.EndDate);
                });

                output.InvalidFinishFinishDependencies = output.InvalidFinishFinishDependencies.Concat(invalidFinishFinishDependencies.ToArray());
            }
        }
    }
}
